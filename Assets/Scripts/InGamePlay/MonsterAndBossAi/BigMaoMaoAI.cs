using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BigMaoMaoAI : MonoBehaviourPun, IDamageable
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
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isAttack, isDead, gethurt;
    public Image hpBar;
    public GameObject body, FlyMaoMao;
    private Material myMaterial;
    private float _disapper;

    private float timer;
    private bool a;
    private GameObject nowCreate;
    public GameObject[] myMonsters;
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
        myself = this.gameObject;
        myTransform = this.GetComponent<Transform>().transform;
        myAnimator = this.GetComponent<Animator>();
        myRigidbody = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        myMaterial.SetFloat("_disapper", _disapper);
        if (TargetTransform == null)
            return;
        dis = (myTransform.position - TargetTransform.position).sqrMagnitude;
        hpBar.fillAmount = currentHealth / maxHealth;
        if (this.transform.rotation.y > 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0)); }
        else if (this.transform.rotation.y < 0) { hpBar.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0)); }
        if (PV.IsMine == false)
            return;
        if (isDead == false && gethurt == false)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(TargetTransform.position - myTransform.position), rotateSpeed * Time.deltaTime);
            myTransform.transform.LookAt(new Vector3(TargetTransform.position.x, myTransform.position.y, 0));
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
                a = true;
            }
            else if (dis < attackdis)
            {
                Attack();
                a = false;
            }
            if (a == true)
            {
                timer += Time.deltaTime;
                if (timer > 5f)
                {
                    for (int i = 0; i < myMonsters.Length; i++)
                    {
                        if (myMonsters[i] == null)
                        {
                            nowCreate = PhotonNetwork.Instantiate(FlyMaoMao.name, myTransform.position + Vector3.up * 5f, Quaternion.Euler(new Vector3(0,-90,0)));
                            timer = 0;
                            a = false;
                            break;
                        }
                    }
                }
            }
            if (myTransform.position.z != 0)
            {
                Vector3 z = new Vector3(myTransform.position.x, myTransform.position.y, 0);
                myTransform.position = Vector3.MoveTowards(myTransform.position, z, 1);
            }
        }
    }
    private void OnCollisionEnter(Collision attack)
    {
        if (isAttack == true)
        {
            if (attack.transform.CompareTag("Player"))
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
    public void Attack()
    {
        isAttack = true;
        myAnimator.SetBool("Idle", false);
        myAnimator.SetBool("Attack", true);
        if (isAttack == true)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|attack") && (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f))
            {
                isAttack = false;
            }
        }
    }
    IEnumerator GetHurt()
    {
        gethurt = true;
        myAnimator.SetTrigger("Hurt");
        myRigidbody.AddForce(-transform.forward * HitForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(2f);
        gethurt = false;
    }
    IEnumerator ReallyDie()
    {
        DOTween.To(() => _disapper, x => _disapper = x, 1.0f, 2);
        yield return new WaitForSeconds(2.5f);
        PhotonNetwork.Destroy(myself);
    }
}