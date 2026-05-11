using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalProgrammerSkill : MonoBehaviour
{
    public GameObject Player;
    private Vector3 cameraTransform;
    private Vector3 rotatePoint;
    public float rotateSpeed, zoomSpeed;
    private bool UseDrag, UseSpin, UseZoom;
    private LayerMask DragObjectLayer;
    public bool isUseSkill;
    private void Awake()
    {
        DragObjectLayer = 1 << (LayerMask.NameToLayer("DragObject"));
        UseDrag = false;
        UseSpin = false;
        UseZoom = false;
        isUseSkill = false;
    }
    private void FixedUpdate()
    {
        cameraTransform = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, DragObjectLayer.value))
        {
            Debug.DrawLine(ray.origin, hit.transform.position, Color.green);
            if (hit.collider.tag == "DragItem")
            {
                var owner = hit.collider.GetComponent<LocalGameobject>();
                if (Input.GetMouseButtonDown(1))
                {
                    owner.useKinematic = true;
                }
                if (Input.GetMouseButton(1))
                {
                    if (UseDrag == true)
                    {
                        hit.collider.transform.position = cameraTransform;
                    }
                    else if (UseSpin == true)
                    {
                        float dis = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * rotateSpeed;
                        rotatePoint = new Vector3(0, 0, dis);
                        hit.collider.transform.Rotate(rotatePoint);
                    }
                    else if (UseZoom == true)
                    {
                        float dis = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * zoomSpeed;
                        float scaleX = owner.scaleX;
                        float scaleY = owner.scaleY;
                        float maxScaleX = scaleX * 2;
                        float minScaleX = scaleX / 2;
                        hit.collider.transform.localScale += new Vector3(dis, dis, 0);
                        if (hit.collider.transform.localScale.x > maxScaleX)
                        {
                            hit.collider.transform.localScale = new Vector3(maxScaleX, scaleY * 2, hit.collider.transform.localScale.z);
                        }
                        else if (hit.collider.transform.localScale.x < minScaleX)
                        {
                            hit.collider.transform.localScale = new Vector3(minScaleX, scaleY / 2, hit.collider.transform.localScale.z);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    owner.useKinematic = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad7)) { int a = 1; SwitchSkill(a); }
        if (Input.GetKeyDown(KeyCode.Keypad8)) { int a = 2; SwitchSkill(a); }
        if (Input.GetKeyDown(KeyCode.Keypad9)) { int a = 3; SwitchSkill(a); }
    }
    public void SwitchSkill(int a)
    {
        if (a == 0)
        {
            UseDrag = false;
            UseSpin = false;
            UseZoom = false;
        }
        else if (a == 1)
        {
            UseDrag = true;
            UseSpin = false;
            UseZoom = false;
        }
        else if (a == 2)
        {
            UseDrag = false;
            UseSpin = true;
            UseZoom = false;
        }
        else if (a == 3)
        {
            UseDrag = false;
            UseSpin = false;
            UseZoom = true;
        }
    }
}
