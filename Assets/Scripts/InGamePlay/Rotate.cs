using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviourPun
{
    private PhotonView PV;
    private Transform This;
    public Vector3 rotationSpeed;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        This = this.transform;
    }
    void Update()
    {
        if (!PV.IsMine)
            return;
        This.Rotate(rotationSpeed * Time.deltaTime);
    }
}