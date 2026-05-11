using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackHitBox : MonoBehaviourPun
{
    public int damage;
    public void OnCollisionEnter(Collision hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            PlayerController player = hitInfo.transform.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }
        else if (hitInfo.gameObject.CompareTag("Player2"))
        {
            PlayerController2 player = hitInfo.transform.GetComponent<PlayerController2>();
            player.TakeDamage(damage);
        }
    }
    public void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            PlayerController player = hitInfo.transform.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }
        else if (hitInfo.gameObject.CompareTag("Player2"))
        {
            PlayerController2 player = hitInfo.transform.GetComponent<PlayerController2>();
            player.TakeDamage(damage);
        }
    }
}