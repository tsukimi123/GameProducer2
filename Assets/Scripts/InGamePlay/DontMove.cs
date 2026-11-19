using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMove : MonoBehaviourPun
{
    private PhotonView PV;
    private Rigidbody RB;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        RB = GetComponent<Rigidbody>();
    }
    void Update()
    {
        RB.linearVelocity = Vector3.zero;
    }
}
