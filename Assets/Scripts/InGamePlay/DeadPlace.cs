using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeadPlace : MonoBehaviourPun
{
    [SerializeField] private bool once;
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.Die();
            if (once == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Player2"))
        {
            PlayerController2 playerController2 = other.GetComponent<PlayerController2>();
            playerController2.Die();
            if (once == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Monster"))
        {
            MonsterAI monsterAI = other.GetComponent<MonsterAI>();
            monsterAI.Die();
        }
    }
}
