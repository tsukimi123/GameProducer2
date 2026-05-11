using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailDetector : MonoBehaviourPun
{
    private PhotonView PV;
    public bool ArtistDead = false, CoderDead = false;
    public Vector3 LastCheckPoint;
    [SerializeField] private GameObject Boss, checkPoint;
    [SerializeField] private SetBossTrue SBT;
    [SerializeField] private GameObject[] Grounds;
    private GameObject Artist, Coder;
    private Vector3[] GroundPosition;
    private float timer;
    public GameObject failText;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        ArtistDead = false;
        CoderDead = false;
        GroundPosition = new Vector3[Grounds.Length];
        for (int i = 0; i < Grounds.Length; i++)
        {
            GroundPosition[i] = Grounds[i].transform.position;
        }
    }
    void Update()
    {
        Coder = GameObject.Find("Player2");
        Artist = GameObject.Find("Player");
        if (Artist != null)
        {
            ArtistDead = Artist.GetComponent<SyncHealth>().isDead;
        }
        if (Coder != null)
        {
            CoderDead = Coder.GetComponent<SyncHealth>().isDead;
        }
        if (ArtistDead && CoderDead)
        {
            timer += Time.deltaTime;
            failText.SetActive(true);
            if (timer > 1.5f)
            {
                failText.SetActive(false);
                PV.RPC("RPC_Flase", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void RPC_Flase()
    {
        for (int i = 0; i < Grounds.Length; i++)
        {
            Grounds[i].GetComponent<Rigidbody>().useGravity = false;
            Grounds[i].transform.position = GroundPosition[i];
        }
        Boss.GetComponent<BossAI>().currentHealth = Boss.GetComponent<BossAI>().maxHealth;
        Boss.GetComponent<BossAI>().aaaaa = 0;
        Boss.SetActive(false);
        checkPoint.SetActive(true);
        SBT.enabled = true;
        GameMaster GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        GM.ArtistPosition = LastCheckPoint;
        GM.CoderPosition = LastCheckPoint;
        PlayerManager PM = GameObject.FindGameObjectWithTag("PM").GetComponent<PlayerManager>();
        PM.Die();
        PlayerManager2 PM2 = GameObject.FindGameObjectWithTag("PM2").GetComponent<PlayerManager2>();
        PM2.Die();
    }
}
