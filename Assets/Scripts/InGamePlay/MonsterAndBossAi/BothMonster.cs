using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothMonster : MonoBehaviourPun
{
    private PhotonView PV;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private BigMaoMaoAI bigMaoMaoAI;
    [SerializeField] private FlyMaomao flyMaomaoAI;
    public bool ground, fly, Big;
    private GameObject player, player2, me;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (ground == true)
        {
            monsterAI = GetComponent<MonsterAI>();
        }
        else if (fly == true)
        {
            flyMaomaoAI = GetComponent<FlyMaomao>();
        }
        else if (Big == true)
        {
            bigMaoMaoAI = GetComponent<BigMaoMaoAI>();
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        me = gameObject;
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player == null && player2 == null)
            return;
        if (player != null && player2 != null)
        {
            float a = (me.transform.position - player.transform.position).sqrMagnitude;
            float b = (me.transform.position - player2.transform.position).sqrMagnitude;
            if (a > b)
            {
                if (ground == true)
                {
                    monsterAI.Target = player2;
                    monsterAI.TargetTransform = player2.transform;
                }
                else if (fly == true)
                {
                    flyMaomaoAI.Target = player2;
                    flyMaomaoAI.TargetTransform = player2.transform;
                }
                else if (Big == true)
                {
                    bigMaoMaoAI.Target = player2;
                    bigMaoMaoAI.TargetTransform = player2.transform;
                }
            }
            else if (a < b)
            {
                if (ground == true)
                {
                    monsterAI.Target = player;
                    monsterAI.TargetTransform = player.transform;
                }
                else if (fly == true)
                {
                    flyMaomaoAI.Target = player;
                    flyMaomaoAI.TargetTransform = player.transform;
                }
                else if (Big == true)
                {
                    bigMaoMaoAI.Target = player;
                    bigMaoMaoAI.TargetTransform = player.transform;
                }
            }
        }
        else if (player == null)
        {
            if (ground == true)
            {
                monsterAI.Target = player2;
                monsterAI.TargetTransform = player2.transform;
            }
            else if (fly == true)
            {
                flyMaomaoAI.Target = player2;
                flyMaomaoAI.TargetTransform = player2.transform;
            }
            else if (Big == true)
            {
                bigMaoMaoAI.Target = player2;
                bigMaoMaoAI.TargetTransform = player2.transform;
            }
        }
        else if (player2 == null)
        {
            if (ground == true)
            {
                monsterAI.Target = player;
                monsterAI.TargetTransform = player.transform;
            }
            else if (fly == true)
            {
                flyMaomaoAI.Target = player;
                flyMaomaoAI.TargetTransform = player.transform;
            }
            else if (Big == true)
            {
                bigMaoMaoAI.Target = player;
                bigMaoMaoAI.TargetTransform = player.transform;
            }
        }
    }
}