using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammerSkill : MonoBehaviourPun
{
    PhotonView PV;
    private Ray ray;
    public GameObject Player;
    public Vector3 cameraTransform;
    public float rotateSpeed, zoomSpeed, bossZ;
    public float throwFroce;
    public Vector3 throwV3;
    public bool UseDrag, UseThrow, UseZoom;
    private bool Holding = false;
    private LayerMask DragObjectLayer;
    public bool isUseSkill, isBossFight;
    public GameObject Effects;
    [SerializeField] private GameObject HitObject, EffectObject;
    private ChangeOwner owner;
    [SerializeField] private CameraController cameraController;
    private int a, b;
    [SerializeField] private SkinnedMeshRenderer[] flyPlane;
    [SerializeField] private Material[] colors;
    private Transform shootPoint;
    private Mouse mouseScript;
    public InputMaster inputMaster;
    private void Awake()
    {
        PV = photonView;
        if (PV.IsMine == false)
        {
            Destroy(this.GetComponent<ProgrammerSkill>());
        }
        inputMaster = new InputMaster();
        inputMaster.Enable();
    }
    private void Start()
    {
        DragObjectLayer = 1 << (LayerMask.NameToLayer("DragObject"));
        UseDrag = true;
        UseThrow = false;
        UseZoom = false;
        isUseSkill = false;
        a = 1;
        mouseScript = GameObject.Find("Mouse").GetComponent<Mouse>();
        UseSkill();
        /*inputMaster.Player.UsingSkill.performed += _ =>
        {
            if (UseZoom == true)
            {
                PhotonNetwork.Instantiate("ScaleBullet", shootPoint.position, shootPoint.rotation);
            }
        };*/
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;
        shootPoint = this.GetComponent<PlayerController2>().shootPoint;
        isBossFight = cameraController.isBossFight;
        if (!isBossFight)
        {
            if (mouseScript.useJoyStick == true)
            {
                cameraTransform = Camera.main.ScreenToWorldPoint(new Vector3(mouseScript.IconPosition.x, mouseScript.IconPosition.y, -Camera.main.transform.position.z));
            }
            else if (mouseScript.useJoyStick == false)
            {
                cameraTransform = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            }
            inputMaster.Player.Skill.performed += _ =>
            {
                if (a == 1) { a = 2; SwitchSkill(a); }
                else if (a == 2) { a = 1; SwitchSkill(a); }
                b += 1;
                if (b >= colors.Length) { b = 0; }
                for (int i = 0; i < flyPlane.Length; i++)
                {
                    flyPlane[i].material = colors[b];
                }
            };
        }
        else if (isBossFight)
        {
            a = 3;
            SwitchSkill(a);
            float z = 20 - Input.mousePosition.y * 0.01f;
            if (mouseScript.useJoyStick == true)
            {
                cameraTransform = Camera.main.ScreenToWorldPoint(new Vector3(mouseScript.IconPosition.x, mouseScript.IconPosition.y, -Camera.main.transform.position.z + z));
            }
            else if (mouseScript.useJoyStick == false)
            {
                cameraTransform = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + z));
            }
        }
        if (UseZoom)
            return;
        if (mouseScript.useJoyStick == true)
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(mouseScript.IconPosition.x, mouseScript.IconPosition.y, -Camera.main.transform.position.z));
        }
        else if (mouseScript.useJoyStick == false)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);
        /*if (Input.GetButtonDown("UsingSkill"))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, DragObjectLayer.value))
            {
                Debug.DrawLine(ray.origin, hit.transform.position, Color.green);
                HitObject = hit.transform.gameObject;
                owner = HitObject.GetComponent<ChangeOwner>();
                if (owner != null)
                {
                    owner.isChangeOwner();
                    EffectObject = PhotonNetwork.Instantiate(Effects.name, hit.transform.position, Quaternion.identity);
                }
            }
        }*/
        if (Holding)
        {
            if (HitObject == null)
                return;
            EffectObject.transform.position = cameraTransform;
            if (UseDrag)
            {
                ChangeOwner CO = HitObject.GetComponent<ChangeOwner>();
                if (CO.DragX)
                {
                    HitObject.GetComponent<Rigidbody>().MovePosition(new Vector3(cameraTransform.x, HitObject.transform.position.y, HitObject.transform.position.z));
                }
                else if (CO.DragY)
                {
                    HitObject.GetComponent<Rigidbody>().MovePosition(new Vector3(HitObject.transform.position.x, cameraTransform.y, HitObject.transform.position.z));
                }
                else if (!CO.DragX)
                {
                    HitObject.GetComponent<Rigidbody>().MovePosition(cameraTransform);
                }
            }
            else if (UseThrow)
            {
                throwV3 = HitObject.transform.position - cameraTransform;
            }
        }
        /*if (Input.GetButtonUp("UsingSkill"))
        {
            if (HitObject == null)
                return;
            if (UseThrow == true)
            {
                HitObject.GetComponent<Rigidbody>().velocity += throwV3 * throwFroce;
            }
            PhotonNetwork.Destroy(EffectObject);
            EffectObject = null;
            HitObject = null;
        }*/
    }
    public void UseSkill()
    {
        inputMaster.Player.UsingSkill.started += _ =>
        {
            if (UseZoom)
                return;
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, DragObjectLayer.value))
            {
                Debug.DrawLine(ray.origin, hit.transform.position, Color.green);
                HitObject = hit.transform.gameObject;
                owner = HitObject.GetComponent<ChangeOwner>();
                if (owner != null)
                {
                    owner.IsChangeOwner();
                    EffectObject = PhotonNetwork.Instantiate(Effects.name, hit.transform.position, Quaternion.identity);
                }
            }
        };
        inputMaster.Player.UsingSkill.performed += _ =>
        {
            if (UseZoom == true)
            {
                PhotonNetwork.Instantiate("ScaleBullet", shootPoint.position, shootPoint.rotation);
                return;
            }
            Holding = true;
        };
        inputMaster.Player.UsingSkill.canceled += _ =>
        {
            if (HitObject == null)
                return;
            if (UseThrow == true)
            {
                HitObject.GetComponent<Rigidbody>().velocity += throwV3 * throwFroce;
            }
            PhotonNetwork.Destroy(EffectObject);
            EffectObject = null;
            HitObject = null;
            Holding = false;
        };
    }
    public void SwitchSkill(int a)
    {
        if (a == 1)
        {
            UseDrag = true;
            UseThrow = false;
            UseZoom = false;
        }
        else if (a == 2)
        {
            UseDrag = false;
            UseThrow = false;
            UseZoom = true;
        }
        else if (a == 3)
        {
            UseDrag = false;
            UseThrow = true;
            UseZoom = false;
        }
    }
    public void OnDestroy()
    {
        inputMaster.Disable();
    }
}