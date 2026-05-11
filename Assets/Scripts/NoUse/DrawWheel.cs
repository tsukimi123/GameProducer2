using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DrawWheel : MonoBehaviourPun
{
    [SerializeField] private int ans, QuestionNum;
    
    private void OnCollisionEnter(Collision collision)
    {
        PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
        if (photonView.IsMine == false)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<InstantiateItems>().nowQusetion = QuestionNum;
            collision.gameObject.GetComponent<InstantiateItems>().answer = ans;
            collision.gameObject.GetComponent<InstantiateItems>().Questions[QuestionNum - 1].SetActive(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
        if (photonView.IsMine == false)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<InstantiateItems>().nowQusetion = 0;
            collision.gameObject.GetComponent<InstantiateItems>().answer = 0;
            collision.gameObject.GetComponent<InstantiateItems>().Questions[QuestionNum - 1].SetActive(false);
        }
    }
}
