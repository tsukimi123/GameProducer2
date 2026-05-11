using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class InstantiateItems : MonoBehaviourPun
{
    PhotonView PV;
    private bool canDrawing;
    [SerializeField] private GameObject me;
    [SerializeField] private GameObject ArtistCanvas;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject skillSetting;
    private ArtistSkill artistSkill;
    private PlayerController playerController = null;
    public GameObject cube, plane;
    public int nowQusetion,drawCheck = 0, answer = 0;
    public int[] QuestionInts;    
    public GameObject[] Questions;
    private LayerMask DrawObjectLayer;
    private Vector3 myPosition;
    private bool setting;
    public GameObject effect;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(this.GetComponent<InstantiateItems>());
        }
        artistSkill = GetComponent<ArtistSkill>();
        playerController = GetComponent<PlayerController>();
        DrawObjectLayer = 1 << (LayerMask.NameToLayer("UI"));
    }
    void Start()
    {
        canDrawing = false;
        ArtistCanvas.SetActive(false);
        skillSetting.SetActive(false);
        drawCheck = 0;
        answer = 0;
        if(PV.IsMine)
        {
            artistSkill.enabled = true;
        }
    }
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetButtonDown("Skill"))
            {
                if (setting == true)
                {
                    skillSetting.SetActive(false);
                    normal.SetActive(true);
                    setting = false;
                }
                else if (setting == false)
                {
                    skillSetting.SetActive(true);
                    normal.SetActive(false);
                    setting = true;
                }
            }
            if (Input.GetButton("UsingSkill"))
            {
                BrushMove();
            }
            else if (Input.GetButtonUp("UsingSkill"))
            {
                ObjInstantiate();
            }
            myPosition = new Vector3(me.transform.position.x, me.transform.position.y + 5f, me.transform.position.z);
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PhotonNetwork.Instantiate("BigPlane", new Vector3(-7f, 78f, 0f), Quaternion.Euler(-90, 90, 0), 0);
                PhotonNetwork.Instantiate(effect.name, new Vector3(-7f, 78f, 0f), Quaternion.Euler(-90, 90, 0), 0);
                PhotonNetwork.Instantiate("elevator", new Vector3(310f, 85f, 0f), Quaternion.identity, 0);
                PhotonNetwork.Instantiate(effect.name, new Vector3(310f, 85f, 0f), Quaternion.identity, 0);
            }
        }
    }
    private void ObjInstantiate()
    {
        if(canDrawing == false)
        {
            if (drawCheck == 1)
            {
                cube = PhotonNetwork.Instantiate("Cube", myPosition, Quaternion.Euler(-90, 0, 0), 0);
                PhotonNetwork.Instantiate(effect.name, myPosition, Quaternion.Euler(-90, 0, 0), 0);
            }
            else if(drawCheck >= 2)
            {
                plane = PhotonNetwork.Instantiate("Plane", myPosition, Quaternion.Euler(-90, 0, 0), 0);
                PhotonNetwork.Instantiate(effect.name, myPosition, Quaternion.Euler(-90, 0, 0), 0);
            }
            else
            {

            }
            Collider[] a = normal.GetComponentsInChildren<Collider>();
            for (int i = 0; i < a.Length; i++)
            {
                a[i].enabled = true;
            }
            drawCheck = 0;
        }
    }
    private void BrushMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f, DrawObjectLayer.value))
        {
            if(hit.collider.CompareTag("ArtistQs"))
            {
                drawCheck += 1;
                hit.collider.enabled = false;
            }
        }
    }
    public void cleanObject()
    {
        if (cube != null)
        {
            cube.GetComponent<ChangeOwner>().IsChangeOwner();
            PhotonNetwork.Destroy(cube);
        }
        if (plane != null)
        {
            plane.GetComponent<ChangeOwner>().IsChangeOwner();
            PhotonNetwork.Destroy(plane);
        }
    }
}