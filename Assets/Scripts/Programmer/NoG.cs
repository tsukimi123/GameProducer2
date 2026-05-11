using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NoG : MonoBehaviourPun
{
    public GameObject[] DragItem;
    void Update()
    {
        
    }
    public void NoGra()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("DragItem");
        DragItem = a;
        for (int i = 0; i < DragItem.Length; i++)
        {
            if (DragItem[i].GetComponent<Rigidbody>() == true)
            {
                DragItem[i].GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
}
