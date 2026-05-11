using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeCubeIn : MonoBehaviourPun
{
    public string targetName, secondName, ThirdName;
    public GameObject plane;
    public Vector3 planePosition, StartPosition, EndPosition;
    public float speed;
    private bool a, b, c;
    private void Start()
    {
        planePosition = plane.transform.position;
    }
    void Update()
    {
        if (a == true && b == true && c == true)
        {
            planePosition = Vector3.MoveTowards(planePosition, EndPosition, speed * Time.deltaTime);
        }
        else
        {
            planePosition = Vector3.MoveTowards(planePosition, StartPosition, speed * Time.deltaTime);
        }
        plane.transform.position = planePosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        ArtWall artWall = other.GetComponent<ArtWall>();
        if (artWall != null)
        {
            string name = artWall.myName;
            if (name == targetName)
            {
                a = true;
            }
            else if (name == secondName)
            {
                b = true;
            }
            else if (name == ThirdName)
            {
                c = true;
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
                a = false;
            }
            else if (name == secondName)
            {
                b = false;
            }
            else if (name == ThirdName)
            {
                c = false;
            }
        }
    }
}
