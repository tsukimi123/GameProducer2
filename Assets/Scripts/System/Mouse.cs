using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject mouseIcon;
    public float moveSpeed = 2f;
    float joystickX, joystickY;
    float screenX, screenY;
    public bool useJoyStick = false;
    public Vector3 a, b;
    public InputMaster inputMaster;
    public Vector2 movement, IconPosition;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        useJoyStick = false;
        screenX = Screen.width / 2;
        screenY = Screen.height / 2;
        inputMaster = new InputMaster();
        inputMaster.Enable();
    }
    void Update()
    {
        a = Input.mousePosition;
        movement = inputMaster.Player.RightStick.ReadValue<Vector2>();
        if (useJoyStick == true) 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            mouseIcon.SetActive(true);
            joystickX = movement.x * moveSpeed;
            joystickY = movement.y * moveSpeed;
            mouseIcon.transform.position = new Vector2(mouseIcon.transform.position.x + joystickX, mouseIcon.transform.position.y + joystickY);
        }
        else
        {
            mouseIcon.transform.position = new Vector2(screenX, screenY);
            mouseIcon.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        IconPosition = mouseIcon.transform.position;
    }
    private void FixedUpdate()
    {
        if (movement.x != 0 || movement.y != 0)
        {
            useJoyStick = true;
        }
        else if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            useJoyStick = false;
        }
    }
}
