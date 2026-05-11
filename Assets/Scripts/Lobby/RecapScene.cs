using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapScene : MonoBehaviourPun
{
    PhotonView PV;
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject StartGameButton;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        Debug.Log(PhotonNetwork.AutomaticallySyncScene);
        if(PhotonNetwork.IsMasterClient == true)
        {
            StartGameButton.SetActive(true);
        }
    }
    public void nextScene()
    {
        PhotonNetwork.LoadLevel(nextSceneName);
    }
}
