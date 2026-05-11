using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Vector3 Target;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = Target;
        }
        else if (other.CompareTag("Player2"))
        {
            other.transform.position = Target;
        }
    }
}
