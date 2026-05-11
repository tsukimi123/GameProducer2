using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    //public Transform target;
    //public Vector3 offset, move;
    //public float smoothSpeed = 0.125f;
    //private Vector3 origin;
    public GameObject Target, firstCamera,secondCamera, bossCamera;
    public CinemachineCameraOffset cinemachineCameraOffset;
    public CinemachineConfiner cinemachineConfiner, cinemachineConfiner2;
    public Vector3 bossCameraPosition;
    public bool isBossFight;
    private void Awake()
    {
        cinemachineCameraOffset = secondCamera.GetComponent<CinemachineCameraOffset>();
        cinemachineConfiner = GetComponent<CinemachineConfiner>();
        cinemachineConfiner2 = secondCamera.GetComponent<CinemachineConfiner>();
        isBossFight = false;
    }
    private void Start()
    {
        cinemachineConfiner.m_BoundingShape2D = GameObject.Find("Confiner").GetComponent<Collider2D>();
        cinemachineConfiner2.m_BoundingShape2D = GameObject.Find("Confiner").GetComponent<Collider2D>();
        bossCamera.transform.position = bossCameraPosition;
    }
    /*private void Update()
    {
        if (isBossFight == true)
        {
            cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            cinemachineConfiner.m_BoundingVolume = GameObject.Find("Confiner3D").GetComponent<BoxCollider>();
            BossOffset.enabled = true;
            this.transform.LookAt(Target.transform);
        }
    }*/
    /*void LateUpdate()
    {
        Vector3 aaa = new Vector3(origin.x, origin.y + move.y, origin.z + move.x);
        target.localPosition = Vector3.MoveTowards(target.localPosition, aaa, smoothSpeed * Time.deltaTime);
        Vector3 ccc = new Vector3(offset.x, offset.y, offset.z + move.z);
        Vector3 desiredPosition = target.position + ccc;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }*/
}