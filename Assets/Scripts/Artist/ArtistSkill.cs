using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class ArtistSkill : MonoBehaviourPun
{
    private GameObject clone;
    private LineRenderer line;
	public float width = 0;
	int i = 0;
	public float ccc = 15;
    //帶有LineRender物體
    public GameObject target;

	public GameObject[] clonelist;
	public List<GameObject> a;
	public Color penColor = Color.red;
	private Mouse mouseScript;
	public InputMaster inputMaster;
	private bool isDrawing;
	private void Awake()
    {
		var PV = GetComponent<PhotonView>();
		if (!PV.IsMine)
		{
			Destroy(this.GetComponent<ArtistSkill>());
		}
		mouseScript = GameObject.Find("Mouse").GetComponent<Mouse>();
		inputMaster = new InputMaster();
		inputMaster.Enable();
    }
    private void Start()
    {
		inputMaster.Player.UsingSkill.started += _ =>
		{
			clone = (GameObject)PhotonNetwork.Instantiate("Draw", target.transform.position, Quaternion.identity, 0);
			a.Add(clone);
			clonelist = a.ToArray();
			line = clone.GetComponent<LineRenderer>();
			line.material = new Material(Shader.Find("Sprites/Default"));
			line.startColor = penColor;
			line.endColor = penColor;
			line.startWidth = width;
			line.endWidth = width / 2;
			i = 0;
		};
		inputMaster.Player.UsingSkill.performed += _ =>
		{
			isDrawing = true;
		};
		inputMaster.Player.UsingSkill.canceled += _ =>
		{
			isDrawing = false;
		};
		inputMaster.Player.Left.performed += _ =>
		{
			ClearDraw();
		};
	}
    private void Update()
    {
		if (isDrawing)
        {
			i++;
			line.positionCount = i;
			if (mouseScript.useJoyStick)
            {
				line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(mouseScript.IconPosition.x, mouseScript.IconPosition.y, ccc)));
			}
			else if(!mouseScript.useJoyStick)
            {
				line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ccc)));
			}
		}
		/*if(mouseScript.useJoyStick == false)
        {
			if (Input.GetMouseButtonDown(1))
			{
				//例項化物件
				clone = (GameObject)PhotonNetwork.Instantiate("Draw", target.transform.position, Quaternion.identity, 0);
				//clone = (GameObject)Instantiate(target, target.transform.position, target.transform.rotation);
				a.Add(clone);
				clonelist = a.ToArray();
				//獲得該物體上的LineRender元件
				line = clone.GetComponent<LineRenderer>();
				line.material = new Material(Shader.Find("Sprites/Default"));
				//設定起始和結束的顏色
				line.startColor = penColor;
				line.endColor = penColor;
				//設定起始和結束的寬度
				line.startWidth = width;
				line.endWidth = width / 2;
				//計數
				i = 0;
			}
			if (Input.GetMouseButton(1))
			{
				//每一幀檢測，按下滑鼠的時間越長，計數越多
				i++;
				//設定頂點數
				line.positionCount = i;
				//設定頂點位置(頂點的索引，將滑鼠點選的螢幕座標轉換為世界座標)
				line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ccc)));
			}
		}
		if (mouseScript.useJoyStick == true)
        {
			if (Input.GetButtonDown("UseSkill"))
			{
				print("GetButtonDown(UseSkill)");
				clone = (GameObject)PhotonNetwork.Instantiate("Draw", target.transform.position, Quaternion.identity, 0);
				a.Add(clone);
				clonelist = a.ToArray();
				line = clone.GetComponent<LineRenderer>();
				line.material = new Material(Shader.Find("Sprites/Default"));
				line.startColor = penColor;
				line.endColor = penColor;
				line.startWidth = width;
				line.endWidth = width / 2;
				i = 0;
			}
			if (Input.GetButton("UseSkill"))
			{
				i++;
				line.positionCount = i;
				line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(mouseScript.mouseIcon.transform.position.x, mouseScript.mouseIcon.transform.position.y, ccc)));
			}
		}*/
		/*if(Input.GetKeyDown(KeyCode.E))
        {
			ClearDraw();
		}*/
	}
	public void ClearDraw()
    {
		a.Clear();
		for (int b = 0; b < clonelist.Length; b++)
		{
			Destroy(clonelist[b]);
		}
	}
	public void OnSliderValueChanged(float value)
    {
		width = value;
    }
	public void ChangeDepth(float value)
    {
		ccc = value;
    }
}