using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBossTrue : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    [SerializeField] private SetBossTrue setBossTrue;
    [SerializeField] private bool ArtistIn, CoderIn;
    [SerializeField] GameObject boss, HpBar, checkPoint;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        setBossTrue = GetComponent<SetBossTrue>();
        boss.SetActive(false);
        HpBar.SetActive(false);
    }
    private void Update()
    {
        if (ArtistIn && CoderIn)
        {
            HpBar.SetActive(true);
            boss.SetActive(true);
            ArtistIn = false;
            CoderIn = false;
            this.setBossTrue.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            PV.RPC("RPC_Artist", RpcTarget.All);
        }
        else if (other.CompareTag("Player2"))
        {
            PV.RPC("RPC_Coder", RpcTarget.All);
        }
    }
    [PunRPC]
    public void RPC_Artist()
    {
        ArtistIn = true;
    }
    [PunRPC]
    public void RPC_Coder()
    {
        CoderIn = true;
    }
}
