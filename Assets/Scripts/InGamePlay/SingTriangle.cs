using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingTriangle : MonoBehaviourPun
{
    private PhotonView PV;
    public GameObject Sing;
    public RectTransform rect;
    public float stayTime, speed;
    public bool a = false;
    public Vector3 SingPosition;
    public Vector3 StartPosition, EndPosition;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        rect = Sing.GetComponent<RectTransform>();
        SingPosition = rect.anchoredPosition;
    }
    private void Update()
    {
        StartCoroutine(Move());
        rect.anchoredPosition = SingPosition;
    }
    IEnumerator Move()
    {
        if (a == true)
        {
            SingPosition = Vector3.MoveTowards(SingPosition, EndPosition, speed * Time.deltaTime);
            if (SingPosition == EndPosition)
            {
                yield return new WaitForSeconds(stayTime);
                a = false;
            }
        }
        else if (a == false)
        {
            SingPosition = Vector3.MoveTowards(SingPosition, StartPosition, speed * Time.deltaTime);
            if (SingPosition == StartPosition)
            {
                yield return new WaitForSeconds(stayTime);
                a = true;
            }
        }
    }
}