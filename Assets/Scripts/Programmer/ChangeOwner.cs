using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOwner : MonoBehaviourPun
{
    public float scaleX, scaleY, scaleZ;
    public bool canScale, DragX, DragY, IHaveRigidBody, gravity;
    private Rigidbody myRigidbody;
    private PhotonView PV;
    private void Awake()
    {
        PV = this.GetComponent<PhotonView>();
        myRigidbody = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(IHaveRigidBody == true)
        {
            if(gravity == false)
            {
                myRigidbody.useGravity = false;
            }
            else if(gravity == true)
            {
                myRigidbody.useGravity = true;
            }
        }
    }
    public void IsChangeOwner()
    {
        base.photonView.RequestOwnership();
    }
}