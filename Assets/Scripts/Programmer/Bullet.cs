using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private PhotonView PV;
    public float damage = 20;
    public float speed = 20f;
    public float destroyTime = 5f;
    public Rigidbody rb;
    public GameObject HitEffect;
    public AudioSource ShootAudio;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        rb.velocity = transform.forward * speed;
        //ShootAudio.Play();
        StartCoroutine(DestroyBullet());
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        if(hitInfo.CompareTag("Player"))
        {
            PlayerController player = hitInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        if(hitInfo.CompareTag("Monster"))
        {
            MonsterAI monsterAI = hitInfo.GetComponent<MonsterAI>();
            FlyMaomao flyMaomao = hitInfo.GetComponent<FlyMaomao>();
            PhotonNetwork.Instantiate(HitEffect.name, hitInfo.transform.position, Quaternion.identity, 0);
            if (monsterAI != null)
            {
                monsterAI.TakeDamage(damage);
                Destroy(this.gameObject);
                monsterAI.GetShoot.Play();
            }
            else if (flyMaomao)
            {
                flyMaomao.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        if (hitInfo.CompareTag("DragItem"))
        {
            ChangeOwner owner = hitInfo.GetComponent<ChangeOwner>();
            if (!owner.canScale)
                return;
            PhotonNetwork.Instantiate(HitEffect.name, hitInfo.transform.position, Quaternion.identity, 0);
            if (owner != null)
            {
                hitInfo.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
                if (hitInfo.transform.localScale.x < owner.scaleX * 0.5f)
                {
                    hitInfo.transform.localScale =new Vector3(owner.scaleX * 0.5f, owner.scaleY * 0.5f, owner.scaleZ * 0.5f);
                }
            }
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
