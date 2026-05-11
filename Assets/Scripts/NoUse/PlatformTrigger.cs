using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlatformTrigger : MonoBehaviourPunCallbacks
{
    Animator PlatformAni;

    private void OnTriggerEnter(Collider other)
    {
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

        if (other.gameObject.tag == "Player")
            PlatformAni.SetBool("starting", true);
        if (other.gameObject.tag == "Player2")
            PlatformAni.SetBool("starting", true);
        print("OAO");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlatformAni.SetBool("starting", false);
        if (other.gameObject.tag == "Player2")
            PlatformAni.SetBool("starting", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlatformAni = this.transform.parent.GetComponentInChildren<Animator>();
    }

    /*[PunRPC]
    void hideCube()
    {
        PlatformAni.SetBool("starting", false);
    }

    [PunRPC]
    public void outCube()
    {
        PlatformAni.SetBool("starting", true);
    }*/
}
