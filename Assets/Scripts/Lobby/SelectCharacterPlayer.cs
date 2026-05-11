using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterPlayer : MonoBehaviourPun
{
    private PhotonView PV;
    public Image myImage;
    public Sprite leftImage, rightImage;
    private RectTransform imageTransform;
    public CharacterSelection2 CS;
    public Vector3 choseAtrist, choseProgrammer;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        imageTransform = myImage.GetComponent<RectTransform>();
    }
    void Start()
    {
        if (!PV.IsMine)
            return;
        CS = GameObject.FindGameObjectWithTag("CS").GetComponent<CharacterSelection2>();
        if (PhotonNetwork.IsMasterClient == true)
        {
            imageTransform.position = new Vector2(Screen.width / 2, Screen.height / 2 + 100);
            choseAtrist += new Vector3(0, 100, 0);
            choseProgrammer += new Vector3(0, 100, 0);
        }
        else
        {
            imageTransform.position = new Vector2(Screen.width / 2, Screen.height / 2 - 100);
            choseAtrist -= new Vector3(0, 100, 0);
            choseProgrammer -= new Vector3(0, 100, 0);
        }
    }

    void Update()
    {
        if (!PV.IsMine)
            return;
        if (CS.index_2 == 1)
        {
            PV.RPC("RPC_Left", RpcTarget.All);
        }
        else if (CS.index_2 == 2)
        {
            PV.RPC("RPC_Right", RpcTarget.All);
        }
    }
    [PunRPC]
    public void RPC_Left()
    {
        myImage.sprite = leftImage;
        myImage.rectTransform.sizeDelta = new Vector2(218, 144);
        if (!PV.IsMine)
            return;
        myImage.rectTransform.position = choseAtrist;
    }
    [PunRPC]
    public void RPC_Right()
    {
        myImage.sprite = rightImage;
        myImage.rectTransform.sizeDelta = new Vector2(218, 144);
        if (!PV.IsMine)
            return;
        myImage.rectTransform.position = choseProgrammer;
    }
}
