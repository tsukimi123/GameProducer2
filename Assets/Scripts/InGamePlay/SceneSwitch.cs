using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneSwitch : MonoBehaviourPun
{
    private PhotonView PV;
    public GameObject[] TargetToOpen, TargetToClose;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var PV = other.GetComponent<PhotonView>();
        if (PV != null && PV.IsMine == true)
        {
            if (other.CompareTag("Player") || other.CompareTag("Player2"))
            {
                for (int i = 0; i < TargetToOpen.Length; i++)
                {
                    TargetToOpen[i].SetActive(true);
                }
                for (int i = 0; i < TargetToClose.Length; i++)
                {
                    TargetToClose[i].SetActive(false);
                }
            }
        }
        else 
            return;
    }
}