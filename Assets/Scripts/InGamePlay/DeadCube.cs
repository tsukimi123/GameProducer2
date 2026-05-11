using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCube : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            artWall.needDestory = false;
            other.GetComponent<ChangeOwner>().gravity = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            artWall.needDestory = false;
            other.GetComponent<ChangeOwner>().gravity = true;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
