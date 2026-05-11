using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDead : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<LocalArtist>().isDead = true;
            other.GetComponent<LocalArtist>().PlayerAni.SetTrigger("Dead");
        }
        if (other.CompareTag("Player2"))
        {
            other.GetComponent<LocalProgrammer>().isDead = true;
            other.GetComponent<LocalProgrammer>().PlayerAni.SetTrigger("Dead");
        }
    }
}
