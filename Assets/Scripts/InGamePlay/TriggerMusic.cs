using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    public AudioSource AS;
    public string TagString;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagString))
        {
            AS.Play();
        }
    }
}
