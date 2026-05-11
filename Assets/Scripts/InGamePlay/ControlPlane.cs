using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlane : MonoBehaviourPun
{
    [SerializeField] private PlaneControllerSetFalse PCSF;
    public string targetName;
    public GameObject target;
    public Vector3 Direction;
    public float speed;
    public bool isIn = false;
    private void Update()
    {
        if (isIn)
        {
            target.transform.position += Direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            string name = artWall.myName;
            if (name == targetName)
            {
                isIn = true;
                PCSF.cube = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            string name = artWall.myName;
            if (name == targetName)
            {
                isIn = false;
            }
        }
    }
}
