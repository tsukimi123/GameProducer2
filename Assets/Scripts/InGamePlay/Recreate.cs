using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recreate : MonoBehaviourPun
{
    private PhotonView PV;
    public float reCreateTime;
    public GameObject create;
    public GameObject[] objects;
    private float[] timer;
    private Vector3[] place;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (!PV.IsMine)
            return;
        timer = new float[objects.Length];
        place = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            place[i] = objects[i].transform.position;
        }
    }
    void Update()
    {
        if (!PV.IsMine)
            return;
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == null)
            {
                timer[i] += Time.deltaTime;
                if (timer[i] >= reCreateTime)
                {
                    objects[i] = PhotonNetwork.Instantiate(create.name, place[i], Quaternion.identity);
                    timer[i] = 0f;
                    break;
                }
            }
        }
    }
}
