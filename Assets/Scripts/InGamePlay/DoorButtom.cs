using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtom : MonoBehaviourPun
{
    private PhotonView PV;
    [SerializeField] private buttomOpenDooer door;
    [SerializeField] private GameObject me;
    [SerializeField] private bool Coder, Artist;
    [SerializeField] private bool Enter = false;
    private float a = 0;
    private void Awake()
    {
        PV =GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (Enter == true)
        {
            a += Time.deltaTime;
            if (a > 0.5f)
            {
                me.transform.position += 5f * Time.deltaTime * Vector3.down;
                if (Artist == true)
                {
                    door.Atrue();
                }
                else if (Coder == true)
                {
                    door.Btrue();
                }
            }
            if (a > 0.8f)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        PhotonView photonView = other.transform.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (Artist == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PV.RPC("RPC_Enter", RpcTarget.All);
            }
        }
        else if (Coder == true)
        {
            if (other.gameObject.CompareTag("Player2"))
            {
                PV.RPC("RPC_Enter", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void RPC_Enter()
    {
        Enter = true;
    }
}