using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviourPun
{
    Rigidbody rb;
    PhotonView PV;
    GameObject Cube;
    public KeyCode PushButton = KeyCode.E;

    [SerializeField]private bool canDraging = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = photonView;
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            OnMouseDrag();
            OnMouseUp();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PushButton) && canDraging)
        {
            if (PV.IsMine)
            {
                Cube = GameObject.FindGameObjectWithTag("DragItem");
                PhotonNetwork.Destroy(Cube);
                PhotonNetwork.Instantiate("Cube (14)", new Vector3(103.9075f, 13.75804f, -5.38512f), Quaternion.identity, 0);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (canDraging)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);

            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.position = objPosition;

            rb.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        rb.isKinematic = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
        {
            canDraging = true;
            print("canDraging!!!");
        }
    }
}
