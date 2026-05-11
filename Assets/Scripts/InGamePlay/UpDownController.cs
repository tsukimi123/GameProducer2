using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownController : MonoBehaviourPun
{
    private PhotonView PV;
    public UpDown UpTarget, downTarget;
    private Vector3 startPosition;
    public float timer, speed, Large;
    [SerializeField] private bool Coder, Artist;
    [SerializeField] private bool Enter = false, Exit = false, startTimer = false;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        startPosition = this.transform.position;
    }
    void Update()
    {
        if (Enter == true)
        {
            if (startTimer == true)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition + Vector3.down, speed * Time.deltaTime);
                }
                if (this.transform.position == startPosition + Vector3.down)
                {
                    timer = 0f;
                    startTimer = false;
                }
            }
        }
        else if (Exit == true)
        {
            if (startTimer == true)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition, speed * Time.deltaTime);
                }
                if (this.transform.position == startPosition)
                {
                    timer = 0f;
                    startTimer = false;
                }
            }
        }
    }
    public void OnCollisionEnter(Collision In)
    {
        PhotonView photonView = In.transform.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (Artist == true)
        {
            if (In.gameObject.CompareTag("Player"))
            {
                PV.RPC("RPC_Enter", RpcTarget.All);
            }
        }
        else if (Coder == true)
        {
            if (In.gameObject.CompareTag("Player2"))
            {
                PV.RPC("RPC_Enter", RpcTarget.All);
            }
        }
    }
    public void OnTriggerExit(Collider exit)
    {
        PhotonView photonView = exit.transform.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (Artist == true)
        {
            if (exit.gameObject.CompareTag("Player"))
            {
                PV.RPC("RPC_Exit", RpcTarget.All);
            }
        }
        else if (Coder == true)
        {
            if (exit.gameObject.CompareTag("Player2"))
            {
                PV.RPC("RPC_Exit", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void RPC_Enter()
    {
        Enter = true;
        Exit = false;
        startTimer = true;
        if (Artist == true)
        {
            UpTarget.a += Large;
            downTarget.a -= Large;
        }
        else if (Coder == true)
        {
            UpTarget.a += Large;
            downTarget.a -= Large;
        }
    }
    [PunRPC]
    public void RPC_Exit()
    {
        Enter = false;
        Exit = true;
        startTimer = true;
    }
}
