using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCheck : MonoBehaviourPun
{
    private PhotonView PV;
    public bool Art, code;
    private GameObject Artist, Coder;
    public float ArtistHealth, CoderHealth;
    public Image otherPlayerHpBar;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (!PV.IsMine)
            Destroy(this);
    }
    void Update()
    {
        if (Art)
        {
            Coder = GameObject.Find("Player2");
            if (Coder != null)
            {
                CoderHealth = Coder.GetComponent<SyncHealth>().myHealth;
                otherPlayerHpBar.fillAmount = CoderHealth / 100f;
            }
        }
        else if (code)
        {
            Artist = GameObject.Find("Player");
            if (Artist != null)
            {
                ArtistHealth = Artist.GetComponent<SyncHealth>().myHealth;
                otherPlayerHpBar.fillAmount = ArtistHealth / 100f;
            }
        }
    }
}
