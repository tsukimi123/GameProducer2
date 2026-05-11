using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bomb : MonoBehaviourPun
{
    private PhotonView PV;
    public GameObject explosionPrefab;
    public float damage;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void OnCollisionEnter(Collision other)
    {
        PhotonView photonView = other.transform.GetComponent<PhotonView>();
        if (other.gameObject.CompareTag("Monster"))
        {
            MonsterAI Ai = other.transform.GetComponent<MonsterAI>();
            FlyMaomao flyMaomao = other.transform.GetComponent<FlyMaomao>();
            BossAI bossAI = other.transform.GetComponent<BossAI>();
            if (Ai != null)
            {
                Ai.TakeDamage(damage);
            }
            else if (flyMaomao == true)
            {
                flyMaomao.TakeDamage(damage);
            }
            else if (bossAI == true)
            {
                bossAI.TakeDamage(damage);
                PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity, 0);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
        if (other.gameObject.CompareTag("BigMaoMao"))
        {
            BigMaoMaoAI Ai = other.transform.GetComponent<BigMaoMaoAI>();
            Ai.TakeDamage(damage);
            PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity, 0);
            PhotonNetwork.Destroy(this.gameObject);
        }
        if (other.gameObject.CompareTag("BreakWall"))
        {
            BreakWall BW = other.transform.GetComponent<BreakWall>();
            if (BW != null)
            {
                BW.DestoryThis();
            }
            PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity, 0);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
