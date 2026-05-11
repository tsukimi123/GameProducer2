using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArtistAttack : MonoBehaviourPun
{
    public float damage = 20;
    public GameObject HitEffect;
    [SerializeField] private GameObject player;
    private PhotonView PV;
    private void Awake()
    {
        PV = player.GetComponent<PhotonView>();
    }
    public void OnTriggerEnter(Collider HitInfo)
    {
        if (!PV.IsMine)
            return;
        ArtWall wall = HitInfo.GetComponent<ArtWall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
        }
        if (HitInfo.CompareTag("Player2"))
        {
            PlayerController2 player = HitInfo.GetComponent<PlayerController2>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        if (HitInfo.CompareTag("Monster"))
        {
            MonsterAI monsterAI = HitInfo.GetComponent<MonsterAI>();
            FlyMaomao flyMaomao = HitInfo.GetComponent<FlyMaomao>();
            PhotonNetwork.Instantiate(HitEffect.name, HitInfo.transform.position, Quaternion.identity, 0);
            if (monsterAI != null)
            {
                monsterAI.TakeDamage(damage);
                monsterAI.GetHit.Play();
            }
            else if (flyMaomao != null)
            {
                flyMaomao.TakeDamage(damage);
                flyMaomao.GetHit.Play();
            }
        }
        /*if (HitInfo.CompareTag("BigMaoMao"))
        {
            BigMaoMaoAI monsterAI = HitInfo.GetComponent<BigMaoMaoAI>();
            PhotonNetwork.Instantiate(HitEffect.name, HitInfo.transform.position, Quaternion.identity, 0);
            if (monsterAI != null)
            {
                monsterAI.TakeDamage(damage);
            }
        }*/
        if(HitInfo.CompareTag("DragItem"))
        {
            var owner = HitInfo.GetComponent<ChangeOwner>();
            if (owner != null)
            {
                if (HitInfo.GetComponent<PhotonView>().IsMine == false)
                {
                    owner.IsChangeOwner();
                }
            }
        }
    }
}