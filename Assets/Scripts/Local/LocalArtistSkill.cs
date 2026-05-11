using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalArtistSkill : MonoBehaviour
{
	private GameObject clone;
	private LineRenderer line;
	public float width = 0;
	int i = 0;
	public float ccc = 15;
	//帶有LineRender物體
	public GameObject target;
	public GameObject Draw;
	public GameObject[] clonelist;
	public List<GameObject> a;
	public Color penColor = Color.red;
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(1))
		{
			//例項化物件
			clone = (GameObject)Instantiate(Draw, target.transform.position, Quaternion.identity);
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
		if (Input.GetKeyDown(KeyCode.E))
		{
			ClearDraw();
		}
	}
	public void ClearDraw()
	{
		a.Clear();
		for (int b = 0; b < clonelist.Length; b++)
		{
			Destroy(clonelist[b]);
		}
	}
}
