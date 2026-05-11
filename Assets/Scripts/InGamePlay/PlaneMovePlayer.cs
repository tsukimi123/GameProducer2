using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovePlayer : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.transform.parent;
            playerController.SetParent(transform);
        }
        else if (other.CompareTag("Player2"))
        {
            var playerController = other.transform.parent;
            playerController.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.transform.parent;
            playerController.SetParent(null);
        }
        else if (other.CompareTag("Player2"))
        {
            var playerController = other.transform.parent;
            playerController.SetParent(null);
        }
    }
}
