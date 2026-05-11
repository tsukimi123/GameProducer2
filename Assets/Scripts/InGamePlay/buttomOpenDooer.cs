using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttomOpenDooer : MonoBehaviourPun
{
    private PhotonView PV;
    [SerializeField]private bool a, b;
    public bool Loop, isLoop = false;
    public GameObject Door;
    public Vector3 end, start, target;
    public float speed, stayTime;
    public AudioSource audioSource;
    private bool AudioPlay;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        start = Door.transform.position;
        target = start;
        AudioPlay = false;
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;
        if (a == true && b == true)
        {
            if (Loop == true)
            {
                StartCoroutine(PlatformMove());
            }
            else if (Loop == false)
            {
                target = Vector3.MoveTowards(Door.transform.position, end, speed * Time.deltaTime);
            }
            Door.transform.position = target;
            if (!AudioPlay)
            {
                audioSource.Play();
                AudioPlay = true;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            PV.RPC("RPC_ATrue", RpcTarget.All);
        }
        else if (other.CompareTag("Player2"))
        {
            PV.RPC("RPC_BTrue", RpcTarget.All);
        }
    }
    public void Atrue()
    {
        PV.RPC("RPC_ATrue", RpcTarget.All);
    }
    public void Btrue()
    {
        PV.RPC("RPC_BTrue", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_ATrue()
    {
        a = true;
    }
    [PunRPC]
    public void RPC_BTrue()
    {
        b = true;
    }
    IEnumerator PlatformMove()
    {
        if (isLoop == true)
        {
            target = Vector3.MoveTowards(target, end, speed * Time.deltaTime);
            if (Door.transform.position == end)
            {
                yield return new WaitForSeconds(stayTime);
                isLoop = false;
            }
        }
        else if (isLoop == false)
        {
            target = Vector3.MoveTowards(target, start, speed * Time.deltaTime);
            if (Door.transform.position == start)
            {
                yield return new WaitForSeconds(stayTime);
                isLoop = true;
            }
        }
    }
}