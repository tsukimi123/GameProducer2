using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateMonster : MonoBehaviourPun
{
    private PhotonView PV;
    public GameObject needToCreateMonster;
    public int monsterQuantity;
    public bool isUnlimitled;
    public Vector3[] myPosition;
    private GameObject nowCreate;
    public GameObject[] myMonsters;
    public List<GameObject> a;
    private float timer;
    private bool aaaaa;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            for (int c = 0; c < monsterQuantity; c++)
            {
                int randomPosition = Random.Range(0, myPosition.Length - 1);
                nowCreate = PhotonNetwork.Instantiate(needToCreateMonster.name, myPosition[randomPosition], Quaternion.Euler(0, -90, 0));
                a.Add(nowCreate);
                myMonsters = a.ToArray();
            }
        }
        else if (PhotonNetwork.IsMasterClient == false)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            if (isUnlimitled == true)
            {
                if (aaaaa == true)
                {
                    for (int i = 0; i < monsterQuantity; i++)
                    {
                        if (myMonsters[i] == null)
                        {
                            int randomPosition = Random.Range(0, myPosition.Length - 1);
                            nowCreate = PhotonNetwork.Instantiate(needToCreateMonster.name, myPosition[randomPosition], Quaternion.Euler(0, -90, 0));
                            a.Add(nowCreate);
                            myMonsters = a.ToArray();
                            aaaaa = false;
                        }
                    }
                }
                timer += Time.deltaTime;
                if (timer >= 5f)
                {
                    aaaaa = true;
                    timer = 0f;
                }
            }
        }
    }
}
