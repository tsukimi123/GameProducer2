using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviourPun
{
    private PhotonView PV;
    public float a, speed;
    public Transform myTransform;
    public Vector3 myPosition;
    void Start()
    {
        myTransform = this.GetComponent<Transform>();
        myPosition = myTransform.position;
    }
    void Update()
    {
        myTransform.position = Vector3.MoveTowards(myTransform.position, myPosition + Vector3.up * a, speed * Time.deltaTime);
        if (a > 30f)
        {
            a = 30f;
        }
        else if (a < -10f)
        {
            a = -10f;
        }
    }
}
