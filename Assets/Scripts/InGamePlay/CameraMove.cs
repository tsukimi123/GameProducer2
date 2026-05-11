using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CameraMove : MonoBehaviourPun
{
    public Vector3 artPosition, programmerPosition;
    public bool moveArtist, moveProgrammer;
    public bool isBoosFight;
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine == false)
        {
            return;
        }
        if (moveArtist == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var cameraController = other.GetComponent<PlayerController>().myCamera.GetComponent<CameraController>();
                cameraController.cinemachineCameraOffset.m_Offset = artPosition;
                cameraController.secondCamera.SetActive(true);
            }
        }
        if (moveProgrammer == true)
        {
            if (other.gameObject.CompareTag("Player2"))
            {
                var cameraController = other.GetComponent<PlayerController2>().myCamera.GetComponent<CameraController>();
                cameraController.cinemachineCameraOffset.m_Offset = programmerPosition;
                cameraController.secondCamera.SetActive(true);
            }
        }
        if (isBoosFight == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var cameraController = other.GetComponent<PlayerController>().myCamera.GetComponent<CameraController>();
                cameraController.isBossFight = true;
                cameraController.bossCamera.SetActive(true);
            }
            if (other.gameObject.CompareTag("Player2"))
            {
                var cameraController = other.GetComponent<PlayerController2>().myCamera.GetComponent<CameraController>();
                cameraController.isBossFight = true;
                cameraController.bossCamera.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
        if (photonView.IsMine == false)
        {
            return;
        }
        if (moveArtist == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var cameraController = other.GetComponent<PlayerController>().myCamera.GetComponent<CameraController>();
                cameraController.secondCamera.SetActive(false);
            }
        }
        if (moveProgrammer == true)
        {
            if (other.gameObject.CompareTag("Player2"))
            {
                var cameraController = other.GetComponent<PlayerController2>().myCamera.GetComponent<CameraController>();
                cameraController.secondCamera.SetActive(false);
            }
        }
        if (isBoosFight == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var cameraController = other.GetComponent<PlayerController>().myCamera.GetComponent<CameraController>();
                cameraController.isBossFight = false;
                cameraController.bossCamera.SetActive(false);
            }
            if (other.gameObject.CompareTag("Player2"))
            {
                var cameraController = other.GetComponent<PlayerController2>().myCamera.GetComponent<CameraController>();
                cameraController.isBossFight = false;
                cameraController.bossCamera.SetActive(false);
            }
        }
    }
}
