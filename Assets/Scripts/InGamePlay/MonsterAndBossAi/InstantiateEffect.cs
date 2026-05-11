using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEffect : MonoBehaviourPun
{
    public GameObject EffectObject, me, target;
    [SerializeField] private PhotonView PV;
    [SerializeField] private Vector3 position;
    private void Awake()
    {
        position = target.transform.position;
    }
    private void Update()
    {
        
    }
    public void OnCollisionEnter(Collision Hitinfo)
    {
        print(Hitinfo.transform.name);
        print("a");
        PhotonNetwork.Instantiate(EffectObject.name, position, Quaternion.identity, 0);
    }
}