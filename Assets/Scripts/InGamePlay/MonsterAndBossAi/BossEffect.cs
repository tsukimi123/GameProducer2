using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffect : MonoBehaviourPun
{
    public float destoryTime;
    public ParticleSystem PS;
    public float damage = 20;
    void Start()
    {
        StartCoroutine(Destory());
    }

    public void OnParticleCollision(GameObject hit)
    {
        if (hit.CompareTag("Player"))
        {
            PlayerController player = hit.transform.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }
        else if (hit.CompareTag("Player2"))
        {
            PlayerController2 player2 = hit.transform.GetComponent<PlayerController2>();
            player2.TakeDamage(damage);
        }
    }

    IEnumerator Destory()
    {
        yield return new WaitForSeconds(destoryTime);
        PhotonNetwork.Destroy(gameObject);
    }
}
