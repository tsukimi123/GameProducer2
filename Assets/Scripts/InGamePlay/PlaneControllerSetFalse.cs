using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControllerSetFalse : MonoBehaviourPun
{
    private PhotonView PV;
    public ControlPlane[] planes;
    public GameObject cube;
    [SerializeField] private GameObject me;
    private Vector3 startPosition;
    public float timer, speed;
    [SerializeField] private bool Artist;
    [SerializeField] private bool Enter = false, Exit = false, startTimer = false;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        startPosition = this.transform.position;
    }
    private void Update()
    {
        if (Enter == true)
        {
            if (startTimer == true)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition + Vector3.down, speed * Time.deltaTime);
                    for (int i = 0; i < planes.Length; i++)
                    {
                        cube.GetComponent<ArtWall>().DestoryTime = 0f;
                    }
                }
                if (this.transform.position == startPosition + Vector3.down)
                {
                    timer = 0f;
                    startTimer = false;
                    Enter = false;
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
                    Exit = false;
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
        if (In.gameObject.CompareTag("Player"))
        {
            PV.RPC("RPC_Enter", RpcTarget.All);
        }
    }
    public void OnTriggerExit(Collider exit)
    {
        PhotonView photonView = exit.transform.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (exit.gameObject.CompareTag("Player"))
        {
            PV.RPC("RPC_Exit", RpcTarget.All);
        }
    }
    [PunRPC]
    public void RPC_Enter()
    {
        Enter = true;
        Exit = false;
        startTimer = true;
    }
    [PunRPC]
    public void RPC_Exit()
    {
        Enter = false;
        Exit = true;
        startTimer = true;
    }
}