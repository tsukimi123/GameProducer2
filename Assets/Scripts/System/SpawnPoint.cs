using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPoint : MonoBehaviourPun
{
    [SerializeField]private GameObject graphics;
    private PhotonView PV;
    private GameMaster GM;
    public bool hasTarget;
    public Vector3 TargetPosition;
    void Awake()
    {
        graphics.SetActive(false);
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (hasTarget == true)
        {
            if (other.gameObject.CompareTag("Player"))
                PV.RPC("RPC_Artist", RpcTarget.All, TargetPosition);
            if (other.gameObject.CompareTag("Player2"))
                PV.RPC("RPC_Coder", RpcTarget.All, TargetPosition);
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
                PV.RPC("RPC_Artist", RpcTarget.All, transform.position);
            if (other.gameObject.CompareTag("Player2"))
                PV.RPC("RPC_Coder", RpcTarget.All, transform.position);
        }
    }
    [PunRPC]
    public void RPC_Artist(Vector3 position)
    {
        GM.ArtistPosition = position;
    }
    [PunRPC]
    public void RPC_Coder(Vector3 position)
    {
        GM.CoderPosition = position;
    }
}
