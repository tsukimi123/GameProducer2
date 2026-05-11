using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager2 : MonoBehaviourPun
{
    PhotonView PV;
    GameObject controller;

     void Awake()
    {
        PV = GetComponent<PhotonView>();  
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreatController2();
        }
    }

    public void CreatController2()
    {
        if (PV.IsMine)
        {
            Vector3 position = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().CoderPosition;
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController2"), position, Quaternion.Euler(0, 90, 0), 0, new object[] { PV.ViewID });
        }
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreatController2();
    }
}