using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviourPun
{
    private PhotonView PV;
    private static GameMaster instance;
    public Vector3 ArtistPosition, CoderPosition;
    public GameObject[] CheckPointPosition;
    private Vector3[] CPP;
    public int nowPosition = 0;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
        CPP = new Vector3[CheckPointPosition.Length];
    }
    private void Start()
    {
        for (int i = 0; i < CPP.Length; i++)
        {
            CPP[i] = CheckPointPosition[i].transform.position;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitckCheckPoint();
        }
    }
    private void SwitckCheckPoint()
    {
        nowPosition += 1;
        if(nowPosition > CPP.Length - 1)
        {
            nowPosition = 0;
        }
        //lastcheckPointPos = CPP[nowPosition];
    }
}