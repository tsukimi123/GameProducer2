using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlatformTriggerCoop : MonoBehaviourPunCallbacks
{
    Animator PlatformAni;
    public GameObject Player1, Player2;
    private void Update()
    {
        if (Player1 != null && Player2 != null)
        {
            PlatformAni.SetBool("starting", true);
        }
        else
        {
            PlatformAni.SetBool("starting", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player1 = other.gameObject;
        }
        if (other.CompareTag("Player2"))
        {
            Player2 = other.gameObject;
        }
        /*//2人跡關門
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.tag == "Player2")
                PlatformAni.SetBool("starting", true);
        }else if(other.gameObject.tag == "Player2")
        {
            if (other.gameObject.tag == "Player")
                PlatformAni.SetBool("starting", true);
        }*/

        /*if (other.gameObject.tag == "Player" && other.gameObject.tag == "Player2")
            PlatformAni.SetBool("starting", true);*/
        /*if (other.gameObject.tag == "Player2")
            PlatformAni.SetBool("starting", true);*/
        print("OAO");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Player1 = null;
        else if (other.CompareTag("Player2"))
            Player2 = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlatformAni = this.transform.parent.GetComponentInChildren<Animator>();
    }
}
