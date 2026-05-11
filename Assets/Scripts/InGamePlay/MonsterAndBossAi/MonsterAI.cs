using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MonsterAI : MonoBehaviourPun, IDamageable
{
    private PhotonView PV;
    private GameObject myself;
    public GameObject Target; //要跟踪的人 
    [SerializeField] private float moveSpeed = 2.0f;//移动速度
    [SerializeField] private float rotateSpeed = 5.0f;//选择速度
    private Transform myTransform;//自己
    public Transform TargetTransform;
    public int damage;
    private Rigidbody myRigidbody;
    [SerializeField] private float dis, maxdis, attackdis;
    private Animator myAnimator;
    public float HitForce;
    [SerializeField] private float maxHealth = 35f;
    private float currentHealth;
    private bool isWaiting, isAttack, isDead, gethurt;
    private float timer;
    public Image hpBar;
    public GameObject body;
    private Material myMaterial;
    private float _disapper;

    public AudioClip mySound;
    public AudioSource myAudioSource, GetHit, GetShoot;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        currentHealth = maxHealth;
    }
    private void Start()
    {
        myMaterial = body.GetComponent<SkinnedMeshRenderer>().material;
        _disapper = 0f;
        if (PV.IsMine == false)
            return;
        currentHealth = maxHealth;
        myself = this.gameObject;
        myTransform = this.GetComponent<Transform>().transform;
        myAnimator = this.GetComponent<Animator>();
        myRigidbody = this.GetComponent<Rigidbody>();
        myAudioSource = this.GetComponent<AudioSource>();
    }
    private void Update()
    {
        myMaterial.SetFloat("_disapper", _disapper);
        if (PV.IsMine == false)
            return;
        if (TargetTransform == null)
            return;
        dis = (myTransform.position - TargetTransform.position).sqrMagnitude;
        hpBar.fillAmount = currentHealth / maxHealth;
        if (this.transform.rotation.y > 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0)); }
        else if (this.transform.rotation.y < 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0)); }
        if (isDead == false)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(TargetTransform.position - myTransform.position), rotateSpeed * Time.deltaTime);
            myTransform.transform.LookAt(new Vector3(TargetTransform.position.x, myTransform.position.y, 0));
            if (gethurt == false)
            {
                if (isWaiting == false)
                {
                    if (dis > maxdis)
                    {
                        myAnimator.SetBool("Idle", true);
                    }
                    else if (dis < maxdis && dis > attackdis)
                    {
                        Debug.DrawLine(Target.transform.position, this.transform.position, Color.yellow);
                        myTransform.position += moveSpeed * Time.deltaTime * myTransform.forward;
                        myAnimator.SetBool("Idle", true);
                        myAnimator.SetBool("Attack", false);
                        isAttack = false;
                        timer = 0;
                    }
                    else if (dis < attackdis)
                    {
                        Attack();
                    }
                }
            }
            if (myTransform.position.z != 0)
            {
                Vector3 z = new Vector3(myTransform.position.x, myTransform.position.y, 0);
                myTransform.position = Vector3.MoveTowards(myTransform.position, z, 1);
            }
            Wait();
        }
    }
    private void OnCollisionEnter(Collision attack)
    {
        if(isAttack == true)
        {
            if(attack.transform.CompareTag("Player"))
            {
                PlayerController player = attack.transform.GetComponent<PlayerController>();
                player.TakeDamage(damage);
            }
            else if (attack.transform.CompareTag("Player2"))
            {
                PlayerController2 player = attack.transform.GetComponent<PlayerController2>();
                player.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        currentHealth -= damage;
        hpBar.fillAmount = currentHealth / maxHealth;
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
    private void Wait()
    {
        if (isWaiting == true)
        {
            timer += Time.deltaTime;
            if (isAttack == true)
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|attack") && (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f))
                {
                    myAnimator.SetBool("Idle", true);
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
    public void Attack()
    {
        isAttack = true;
        isWaiting = true;
        myRigidbody.AddForce((transform.forward + Vector3.up * 0.5f) * 9f, ForceMode.VelocityChange);
        myAnimator.SetBool("Idle", false);
        myAnimator.SetBool("Attack", true);
    }
    IEnumerator GetHurt()
    {
        gethurt = true;
        myAnimator.SetTrigger("Hurt");
        //myTransform.localPosition -= myTransform.forward * moveSpeed * 1f;
        myRigidbody.AddForce(-transform.forward * HitForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(2f);
        gethurt = false;
    }
    IEnumerator ReallyDie()
    {
        DOTween.To(() => _disapper, x => _disapper = x, 1.0f, 1.5f);
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(myself);
    }
}