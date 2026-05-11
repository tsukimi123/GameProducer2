using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameobject : MonoBehaviour
{
    public float scaleX, scaleY;
    public bool IHaveRigidBody, useKinematic;
    private Rigidbody myRigidbody;
    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (IHaveRigidBody == true)
        {
            if (useKinematic == false)
            {
                myRigidbody.isKinematic = false;
            }
            else if (useKinematic == true)
            {
                myRigidbody.isKinematic = true;
            }
        }
    }
}
