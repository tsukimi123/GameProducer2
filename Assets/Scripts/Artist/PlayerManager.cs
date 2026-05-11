using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviourPun
{
    PhotonView PV;
    //CharacterSelection CS;
    //private GameMaster GM;
    GameObject controller;

     void Awake()
    {
        PV = GetComponent<PhotonView>();  
        //CS = GetComponent<CharacterSelection>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreatController();
        }
    }

    // Instantiated our Player Controller
    public void CreatController()
    {
        if (PV.IsMine)
        {
            //Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            Vector3 position = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().ArtistPosition;
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), position, Quaternion.Euler(0,90,0), 0, new object[] { PV.ViewID });
        }
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreatController();
    }
}
