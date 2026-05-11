using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkill : MonoBehaviour
{
    public GameObject isSelect;
    private void OnTriggerStay2D(Collider2D inside)
    {
        isSelect = inside.gameObject;
        inside.gameObject.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height / 2 - 400, 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isSelect = null;
    }
}
