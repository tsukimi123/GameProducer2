using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInstantiateItems : MonoBehaviour
{
    private GameObject Wheel;
    private bool canDrawing;
    [SerializeField] private GameObject me;
    [SerializeField] private GameObject ArtistCanvas;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject skillSetting;
    private ArtistSkill artistSkill;
    private PlayerController playerController = null;
    public GameObject Cube, Plane, BigPlane, elevator;
    public GameObject cube, plane;
    public int nowQusetion, drawCheck = 0, answer = 0;
    public int[] QuestionInts;
    public GameObject[] Questions;
    private LayerMask DrawObjectLayer;
    private Vector3 myPosition;
    private bool setting;
    void Awake()
    {
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
    }
    private void Update()
    {
        /*if (Input.GetButtonDown("Skill"))
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
        if (Input.GetMouseButton(1) || Input.GetButton("UseSkill"))
        {
            BrushMove();
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("UseSkill"))
        {
            ObjInstantiate();
        }*/
        myPosition = new Vector3(me.transform.position.x, me.transform.position.y + 5f, me.transform.position.z);
        if (Input.GetKeyDown(KeyCode.Y)) { cube = Instantiate(Cube, myPosition, Quaternion.Euler(-90, 0, 0)); }
        if (Input.GetKeyDown(KeyCode.H)) { plane = Instantiate(Plane, myPosition, Quaternion.Euler(-90, 0, 0)); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Instantiate(BigPlane, new Vector3(-7f, 78f, 0f), Quaternion.Euler(-90, 90, 0)); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { Instantiate(elevator, new Vector3(300f, 70f, 0f), Quaternion.identity); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DrawPlace")
        {
            canDrawing = true;
            ArtistCanvas.SetActive(true);
            normal.SetActive(false);
            playerController.PlayerAni.SetBool("useskill", true);
            print("canDrawing!!!");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "DrawPlace")
        {
            canDrawing = false;
            ArtistCanvas.SetActive(false);
            normal.SetActive(true);
            playerController.PlayerAni.SetBool("useskill", false);
            for (int a = 0; a < Questions.Length; a++)
            {
                Collider[] b = Questions[a].GetComponentsInChildren<Collider>();
                for (int i = 0; i < QuestionInts[a]; i++)
                {
                    b[i].enabled = true;
                }
            }
            drawCheck = 0;
        }
    }
    private void ObjInstantiate()
    {
        if (canDrawing == false)
        {
            if (drawCheck == 1)
            {
                cube = Instantiate(Cube, myPosition, Quaternion.Euler(-90, 0, 0));
            }
            else if (drawCheck >= 2)
            {
                plane = Instantiate(Plane, myPosition, Quaternion.Euler(-90, 0, 0));
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
        if (drawCheck == answer)
        {
            if (nowQusetion == 1)
            {
                var bp = GameObject.Find("BigPlane");
                if (bp != null)
                {
                    Destroy(bp);
                }
                Instantiate(BigPlane, new Vector3(-7f, 78f, 0f), Quaternion.Euler(-90, 90, 0));
                Collider[] a = Questions[nowQusetion - 1].GetComponentsInChildren<Collider>();
                for (int i = 0; i < QuestionInts[nowQusetion - 1]; i++)
                {
                    a[i].enabled = true;
                }
            }
            else if (nowQusetion == 2)
            {
                Instantiate(elevator, new Vector3(300f, 70f, 0f), Quaternion.identity);
                Collider[] a = Questions[nowQusetion - 1].GetComponentsInChildren<Collider>();
                for (int i = 0; i < QuestionInts[nowQusetion - 1]; i++)
                {
                    a[i].enabled = true;
                }
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
            if (hit.collider.tag == "ArtistQs")
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
            Destroy(cube);
        }
        if (plane != null)
        {
            plane.GetComponent<ChangeOwner>().IsChangeOwner();
            Destroy(plane);
        }
    }
}
