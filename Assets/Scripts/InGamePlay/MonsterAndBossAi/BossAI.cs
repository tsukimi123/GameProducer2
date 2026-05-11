using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Video;
using UnityEngine.UI;

public class BossAI : MonoBehaviourPun, IDamageable
{
    private PhotonView PV;
    public float maxHealth = 500f;
    public float currentHealth;
    private int RandomAction;
    public bool Waiting, a, letFight, isDead, isHurt;
    private float timer, hurtTimer, GTimer;
    public Animator BossAnimator;
    public Image hpBar;
    private AnimatorStateInfo BossAniInfo;
    public Transform shootPoint;
    public float frameCount;
    public GameObject LeftHandHitBox, RightHandHitBox;
    private bool setEffectTrue = false;
    [SerializeField] private VideoPlayer BossFace;
    [SerializeField] private VideoClip StartFace, normalFace, DeadFace;
    [SerializeField] private GameObject[] textObject;
    [SerializeField] private InstantiateEffect bossEffect;
    public GameObject cameraMove, faceLight;
    public GameObject[] Grounds;
    public int aaaaa = 0;
    public AudioSource audioSource;
    public AudioClip AC;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        BossAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        Waiting = true;
        a = false;
        letFight = false;
        isDead = false;
        GTimer = 0;
        LeftHandHitBox.SetActive(false);
        RightHandHitBox.SetActive(false);
    }
    private void Update()
    {
        hpBar.fillAmount = currentHealth / maxHealth;
        if (isHurt == true)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer >= 1.5f)
            {
                hurtTimer = 0;
                isHurt = false;
            }
        }
        if (!PV.IsMine)
            return;
        BossAniInfo = BossAnimator.GetCurrentAnimatorStateInfo(0);
        if (letFight == false)
        {
            if (BossAniInfo.IsName("Base Layer.BOSS|BossStartshow"))
            {
                //PV.RPC("RPC_SyncHealth", RpcTarget.All);
                if (BossAniInfo.normalizedTime > 0.95f)
                {
                    PV.RPC("RPC_FUCK", RpcTarget.All);
                }
            }
        }
        if (isDead == true)
        {
            Dead();
        }
        else if (isDead == false)
        {
            if (letFight == true)
            {
                BossFace.clip = normalFace;
                BossActions();
                if (aaaaa < Grounds.Length)
                {
                    aaaaa = (int)((maxHealth - currentHealth) / 10f) - 1;
                    GTimer += Time.deltaTime;
                    if (GTimer > 1f &&　aaaaa >= 0)
                    {
                        for (int i = 0; i <= aaaaa; i++)
                        {
                            Grounds[i].GetComponent<Rigidbody>().useGravity = true;
                            GTimer = 0f;
                        }
                    }
                }
                var randomTarget = Random.Range(0, 2);
                if (randomTarget == 0)
                {
                    var player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        shootPoint.LookAt(player.transform);
                    }
                }
                else if (randomTarget == 1)
                {
                    var player2 = GameObject.FindGameObjectWithTag("Player2");
                    if (player2 != null)
                    {
                        shootPoint.LookAt(player2.transform);
                    }
                }
            }
        }
    }
    private void BossActions()
    {
        
        if (!BossAniInfo.IsName("Base Layer.BOSS|BossIdle") && !BossAniInfo.IsName("Base Layer.BOSS|BossBeHurt"))
        {
            if (BossAniInfo.normalizedTime > 0.4f)
            {
                LeftHandHitBox.SetActive(true);
                RightHandHitBox.SetActive(true);
                if (setEffectTrue == true)
                {
                    bossEffect.enabled = true;
                }
            }
            if (BossAniInfo.normalizedTime > 0.8f)
            {
                BossAnimator.SetBool("LeftAttackSpin", false);
                BossAnimator.SetBool("LeftAttack2Hits", false);
                BossAnimator.SetBool("RightAttack", false);
                BossAnimator.SetBool("RightAttack2", false);
                BossAnimator.SetBool("RightAttack2S", false);
                bossEffect.enabled = false;
                setEffectTrue = false;
            }
            if (BossAniInfo.normalizedTime > 0.9f)
            {
                Waiting = true;
                a = true;
                LeftHandHitBox.SetActive(false);
                RightHandHitBox.SetActive(false);
            }
        }
        else if (BossAniInfo.IsName("Base Layer.BOSS|BossBeHurt"))
        {
            Waiting = true;
            if (BossAniInfo.normalizedTime > 0.75f)
            {
                BossAnimator.SetBool("BeHurt", false);
                if (Waiting == true)
                {
                    RandomAction = Random.Range(1, 6);
                    PV.RPC("RPC_SyncAnimation", RpcTarget.All, RandomAction);
                    Waiting = false;
                }
            }
        }
        else if (BossAniInfo.IsName("Base Layer.BOSS|BossIdle"))
        {
            timer += Time.deltaTime;
            if (timer > 1.25f)
            {
                int c = Random.Range(0, 10);
                Shoot(c);
                timer = 0f;
            }
            if (a == true)
            {
                int b = Random.Range(0, 2);
                if (b == 0)
                {
                    PV.RPC("RPC_SyncFace", RpcTarget.All, 0);
                }
                else if (b == 1)
                {
                    PV.RPC("RPC_SyncFace", RpcTarget.All, 72);
                }
                a = false;
            }
            if (BossAniInfo.normalizedTime > 0.9f)
            {
                if (Waiting == true)
                {
                    RandomAction = Random.Range(1, 6);
                    PV.RPC("RPC_SyncAnimation", RpcTarget.All, RandomAction);
                    Waiting = false;
                }
            }
        }
    }
    private void LeftAttack()
    {
        BossAnimator.SetBool("LeftAttackSpin", true);
        RandomAction = 0;
        PV.RPC("RPC_SyncFace", RpcTarget.All, 144);
    }
    private void LeftAttack2Hits()
    {
        BossAnimator.SetBool("LeftAttack2Hits", true);
        RandomAction = 0;
        PV.RPC("RPC_SyncFace", RpcTarget.All, 192);
    }
    private void RightAttack()
    {
        BossAnimator.SetBool("RightAttack", true);
        setEffectTrue = true;
        RandomAction = 0;
        PV.RPC("RPC_SyncFace", RpcTarget.All, 240);
    }
    private void RightAttack2()
    {
        BossAnimator.SetBool("RightAttack2", true);
        RandomAction = 0;
        PV.RPC("RPC_SyncFace", RpcTarget.All, 288);
    }
    private void RightAttack2S()
    {
        BossAnimator.SetBool("RightAttack2S", true);
        RandomAction = 0;
        PV.RPC("RPC_SyncFace", RpcTarget.All, 335);
    }
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (isHurt)
            return;
        currentHealth -= damage;
        hpBar.fillAmount = currentHealth / maxHealth;
        isHurt = true;
        audioSource.Play();
        if (currentHealth <= 0)
        {
            isDead = true;
        }
        BossAnimator.SetBool("BeHurt", true);
    }
    public void Dead()
    {
        BossAnimator.SetBool("Dead", true);
        if (BossAniInfo.IsName("Base Layer.BOSS|BossDead"))
        {
            if (BossAniInfo.normalizedTime > 0.7f)
            {
                PV.RPC("RPC_SetLightTrue", RpcTarget.All);
                cameraMove.transform.position += 5f * Time.deltaTime * Vector3.down;
                PV.RPC("RPC_Dead", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void RPC_SyncAnimation(int RA)
    {
        if (RA == 1)
        {
            LeftAttack();
        }
        else if (RA == 2)
        {
            LeftAttack2Hits();
        }
        else if (RA == 3)
        {
            RightAttack();
        }
        else if (RA == 4)
        {
            RightAttack2();
        }
        else if (RA == 5)
        {
            RightAttack2S();
        }
    }
    [PunRPC]
    public void RPC_Dead()
    {
        BossFace.isLooping = false;
        BossFace.clip = DeadFace;
    }
    [PunRPC]
    public void RPC_SyncFace(int frame)
    {
        BossFace.frame = frame;
    }
    [PunRPC]
    public void RPC_SetLightTrue()
    {
        faceLight.SetActive(true);
    }
    [PunRPC]
    public void RPC_FUCK()
    {
        letFight = true;
        Waiting = true;
        BossFace.clip = normalFace;
    }
    private void Shoot(int number)
    {
        if (PV.IsMine)
            PhotonNetwork.Instantiate(textObject[number].name, shootPoint.position, shootPoint.rotation);
    }
}