using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    //public Transform target;
    //public Vector3 offset, move;
    //public float smoothSpeed = 0.125f;
    //private Vector3 origin;
    public GameObject Target, firstCamera,secondCamera, bossCamera;
    public Unity.Cinemachine.CinemachineCameraOffset cinemachineCameraOffset;
    // Cinemachine 3：舊的 CinemachineConfiner（2D 模式）由 Upgrader 轉換成 CinemachineConfiner2D
    public Unity.Cinemachine.CinemachineConfiner2D cinemachineConfiner, cinemachineConfiner2;
    public Vector3 bossCameraPosition;
    public bool isBossFight;
    private void Awake()
    {
        cinemachineCameraOffset = secondCamera.GetComponent<Unity.Cinemachine.CinemachineCameraOffset>();
        cinemachineConfiner = GetComponent<Unity.Cinemachine.CinemachineConfiner2D>();
        cinemachineConfiner2 = secondCamera.GetComponent<Unity.Cinemachine.CinemachineConfiner2D>();
        isBossFight = false;
    }
    private void Start()
    {
        var confinerObject = GameObject.Find("Confiner");
        var shape = confinerObject != null ? confinerObject.GetComponent<Collider2D>() : null;
        SetBoundingShape(cinemachineConfiner, shape);
        SetBoundingShape(cinemachineConfiner2, shape);
        bossCamera.transform.position = bossCameraPosition;
    }
    private static void SetBoundingShape(Unity.Cinemachine.CinemachineConfiner2D confiner, Collider2D shape)
    {
        // 還沒跑 Cinemachine Upgrader 時 prefab 上沒有 Confiner2D，略過而不是丟 NullReferenceException
        if (confiner == null)
            return;
        confiner.BoundingShape2D = shape;
        confiner.InvalidateBoundingShapeCache();
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