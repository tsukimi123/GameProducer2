using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviourPun
{
    private Vector3 me;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DragItem"))
        {
            other.transform.position = transform.position;
        }
    }
}
