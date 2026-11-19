using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CameraIsMine : MonoBehaviour
{
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            // 用共同基底類別：Cinemachine Upgrader 轉換前（CinemachineVirtualCamera）和轉換後（CinemachineCamera）都找得到
            var vcam = GetComponentInChildren<Unity.Cinemachine.CinemachineVirtualCameraBase>();
            if (vcam != null)
                Destroy(vcam.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
    }
}
