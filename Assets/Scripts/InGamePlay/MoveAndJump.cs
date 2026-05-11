using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndJump : MonoBehaviourPun
{
    private PhotonView PV;
    [SerializeField] private GameObject plane;
    public float one, two;
    void Update()
    {
        plane.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (plane.transform.localPosition.x < one)
        {
            plane.transform.localPosition = new Vector3(one * 0.99f, 0, 0);
        }
        else if (plane.transform.localPosition.x > two)
        {
            plane.transform.localPosition = new Vector3(two * 0.99f, 0, 0);
        }
    }
    /*public void OnTriggerEnter(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (plane != null)
            return;
        if (artWall != null)
        {
            if (artWall.myName != "Plane")
                return;
            Destroy(artWall);
            plane = other.gameObject;
            plane.transform.parent = gameObject.transform;
            plane.transform.localScale = new Vector3(0.03f, 0.3333333f, 1f);
            plane.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            plane.transform.localPosition = Vector3.zero;
            other.GetComponent<ChangeOwner>().gravity = false;
            //other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }*/
}
