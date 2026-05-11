using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeDoor : MonoBehaviourPun
{
    [SerializeField] private GameObject Door;

    private void Awake()
    {
        Door.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Door.SetActive(true);
        }
    }
}
