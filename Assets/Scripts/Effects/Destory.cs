using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destory : MonoBehaviourPun
{
    private PhotonView PV;
    public float destoryTime;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (!PV.IsMine)
            return;
        StartCoroutine(destory());
    }
    IEnumerator destory()
    {
        yield return new WaitForSeconds(destoryTime);
        PhotonNetwork.Destroy(gameObject);
    }
}
