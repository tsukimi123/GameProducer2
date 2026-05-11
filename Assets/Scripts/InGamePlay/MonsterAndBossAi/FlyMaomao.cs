using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlyMaomao : MonoBehaviourPun, IDamageable
{
    private PhotonView PV;
    private GameObject myself;
    public GameObject Target; //要跟踪的人 
    [SerializeField] private float moveSpeed = 2.0f;//移动速度
    private Transform myTransform;//自己
    public Transform TargetTransform;
    public int D;
    [SerializeField] private float dis, attackdis;
    public Vector3 LoopPosition1, LoopPosition2;
    private Vector3 movePosition, TargetPosition;
    private Animator myAnimator;
    private const float maxHealth = 19f;
    private float currentHealth;
    private bool isWaiting, isAttack, isDead, gethurt;
    private float timer;
    public Image hpBar;
    public GameObject body, shootPoint, bullet, groundMaoMao;
    private Material myMaterial;
    private float _disapper;

    public AudioClip mySound;
    public AudioSource myAudioSource, GetHit, GetShoot;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        GetComponent<Rigidbody>().useGravity = false;
        currentHealth = maxHealth;
    }
    private void Start()
    {
        myMaterial = body.GetComponent<SkinnedMeshRenderer>().material;
        _disapper = 0f;
        if (PV.IsMine == false)
            return;
        myself = this.gameObject;
        moveSpeed += Random.Range(0, 3);
        myTransform = this.GetComponent<Transform>().transform;
        myAnimator = this.GetComponent<Animator>();
        movePosition = myTransform.position;
        LoopPosition1 = myTransform.position + Vector3.right * D;
        LoopPosition2 = myTransform.position + Vector3.left * D;
        TargetPosition = LoopPosition2;
    }
    private void Update()
    {
        myMaterial.SetFloat("_disapper", _disapper);
        hpBar.fillAmount = currentHealth / maxHealth;
        dis = (myTransform.position - TargetTransform.position).sqrMagnitude;
        if (this.transform.rotation.y > 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0)); }
        else if (this.transform.rotation.y < 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0)); }
        if (PV.IsMine == false)
            return;
        if (isDead == false)
        {
            movePosition = Vector3.MoveTowards(movePosition, TargetPosition, moveSpeed * Time.deltaTime);
            if (movePosition == LoopPosition1) { TargetPosition = LoopPosition2; myTransform.rotation = Quaternion.Euler(new Vector3(0, -90, 0)); }
            else if (movePosition == LoopPosition2) { TargetPosition = LoopPosition1; myTransform.rotation = Quaternion.Euler(new Vector3(0, 90, 0)); }
            if (gethurt == false)
            {
                if (dis > attackdis)
                {
                    myAnimator.SetBool("Fly", true);
                    myAnimator.SetBool("Attack", false);
                    isAttack = false;
                    timer = 0;
                }
                else if (dis < attackdis)
                {
                    if (isWaiting == false)
                    {
                        Attack();
                    }
                }
                if (myTransform.position.z != 0)
                {
                    Vector3 z = new Vector3(myTransform.position.x, myTransform.position.y, 0);
                    myTransform.position = Vector3.MoveTowards(myTransform.position, z, 1);
                }
            }
            myTransform.position = movePosition;
        }
        shootPoint.transform.LookAt(TargetTransform);
        Wait();
    }
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(GetHurt());
        Debug.Log("took damage: " + damage);
    }
    public void Die()
    {
        isDead = true;
        myAnimator.SetBool("Dead", true);
        StartCoroutine(ReallyDie());
    }
    public void Attack()
    {
        isAttack = true;
        isWaiting = true;
        myAnimator.SetBool("Fly", false);
        myAnimator.SetBool("Attack", true);
    }
    private void Wait()
    {
        if (isWaiting == true)
        {
            timer += Time.deltaTime;
            if (isAttack == true)
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|attack") && (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f))
                {
                    int a = Random.Range(1, 5);
                    if (a == 4)
                    {
                        PhotonNetwork.Instantiate(groundMaoMao.name, shootPoint.transform.position, shootPoint.transform.rotation);
                    }
                    else if (a > 0 && a <= 3)
                    {
                        PhotonNetwork.Instantiate(bullet.name, shootPoint.transform.position, shootPoint.transform.rotation);
                    }
                    myAnimator.SetBool("Fly", true);
                    myAnimator.SetBool("Attack", false);
                    isAttack = false;
                }
            }
        }
        if (timer >= 2f)
        {
            isWaiting = false;
            timer = 0;
        }
    }
    IEnumerator GetHurt()
    {
        gethurt = true;
        myAnimator.SetTrigger("Hurt");
        yield return new WaitForSeconds(1.5f);
        gethurt = false;
    }
    IEnumerator ReallyDie()
    {
        DOTween.To(() => _disapper, x => _disapper = x, 1.0f, 1.5f);
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(myself);
    }
}