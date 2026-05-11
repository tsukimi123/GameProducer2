using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviourPun
{
    private PhotonView PV;
    [SerializeField] private GameObject me, target;
    [SerializeField]private Vector3 myPositon, targetPositon;
    private Transform myTransform, targetTransform;
    public float Large;
    [SerializeField] private float allMass, targetMass;
    public MovePlane MP;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        myPositon = me.transform.position;
        myTransform = me.transform;
        targetTransform = target.transform;
        Destroy(MP);
    }
    void Update()
    {
        myTransform.position = Vector3.MoveTowards(myTransform.position, myPositon + allMass * Vector3.down, 5f * Time.deltaTime);
        targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPositon + allMass * Large * Vector3.up, 5f * Time.deltaTime); 
        if (allMass >= targetMass)
        {
            allMass = targetMass;
        }
        else if (allMass < 0)
        {
            allMass = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody RB = other.GetComponent<Rigidbody>();
        if (RB != null)
        {
            allMass += RB.mass * 0.2f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody RB = other.GetComponent<Rigidbody>();
        if (RB != null)
        {
            allMass -= RB.mass * 0.2f;
        }
    }
}
