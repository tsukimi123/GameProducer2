using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncHealth : MonoBehaviourPun, IPunObservable
{
    private PhotonView PV;
    public bool artist, coder;
    public bool isDead, netDead;
    private PlayerController PC;
    private PlayerController2 PC2;
    public float myHealth, netHealth;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (!PV.IsMine)
            return;
        isDead = false;
        netDead = false;
        if (artist)
        {
            PC = GetComponent<PlayerController>();
        }
        else if (coder)
        {
            PC2 = GetComponent<PlayerController2>();
        }
    }
    void Update()
    {
        if (PV.IsMine)
        {
            if (artist)
            {
                myHealth = PC.currentHealth;
                isDead = PC.isDead;
            }
            else if (coder)
            {
                myHealth = PC2.currentHealth;
                isDead = PC2.isDead;
            }
        }
        else if (!PV.IsMine)
        {
            myHealth = netHealth;
            isDead = netDead;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myHealth);
            stream.SendNext(isDead);
        }
        else
        {
            this.netHealth = (float)stream.ReceiveNext();
            this.netDead = (bool)stream.ReceiveNext();
        }
    }
}
