using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearShake : MonoBehaviourPun
{
    private PhotonView PV;
    private Transform squareTransform;
    public float shakeTime = 0.2f;//震動時間
    public float shakeAmount = 0.7f;//振幅
    public float decreaseFactor = 1.0f;
    public float under;
    public Vector3 originalPos, StartCenter;
    public bool a = false;
    private HingeJoint HJ;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        squareTransform = GetComponent<Transform>();
    }
    void Start()
    {
        HJ = GetComponent<HingeJoint>();
        StartCenter = HJ.connectedAnchor;
    }
    void Update()
    {
        originalPos = squareTransform.transform.position;//這條語句應該放在落地判斷完畢之後
        if (a && shakeTime > 0)
        {
            squareTransform.position = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeTime -= Time.deltaTime * decreaseFactor;
        }
        if (shakeTime <= 0)
        {
            HJ.connectedAnchor += 25f * Time.deltaTime * Vector3.down;
            this.GetComponent<Collider>().isTrigger = true;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (HJ.connectedAnchor.y < under)
        {
            HJ.connectedAnchor = StartCenter;
            this.GetComponent<Collider>().isTrigger = false;
            shakeTime = 1.75f;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            a = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            a = true;
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            a = true;
        }
    }
}