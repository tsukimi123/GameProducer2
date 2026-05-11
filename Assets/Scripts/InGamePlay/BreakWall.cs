using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviourPun
{
    private PhotonView PV;
    public GameObject breakWall;
    private Vector3 myPosition;
    private Quaternion myRotation;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        myPosition = this.transform.position;
        myRotation = this.transform.rotation;
    }
    public void DestoryThis()
    {
        PV.RPC("RPC_DestoryThis", RpcTarget.All);
    }
    [PunRPC]
    void RPC_DestoryThis()
    {
        PhotonNetwork.Instantiate(breakWall.name, myPosition, myRotation);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
