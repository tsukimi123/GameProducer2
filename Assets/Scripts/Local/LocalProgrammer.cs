using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalProgrammer : MonoBehaviour
{
    public GameObject ProgrammerCanvas;
    public float moveSpeed;
    public Rigidbody playerRigidbody;
    public float jumpForce;
    public float gravityScale;
    public float fallMultiplier = 2.5f;
    public float dashSpeed;
    public float dashTime;
    public float rotateSpeed;

    private bool canDash = true;
    private bool isGrounded = false;
    private bool isOnWall = false;
    public float distantToGround = 1f;
    public float distantToWall = 1f;
    private float horizontalInput;
    private int extraJump;
    public int extraJumpValue;

    private int extraDash;
    public int extraDashValue;

    const float maxHealth = 100f;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public Animator PlayerAni;
    [SerializeField] ProgrammerSkill programmerSkill;
    private bool isUseSkill = false;
    public bool isDead;
    private float faceLeft;
    public GameObject switchSkillCanvas, useSkillCanvas;
    public GameObject myCamera, camTarget;
    [SerializeField] private GameObject body;
    public GameObject PS, SP;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        PlayerAni = GetComponentInChildren<Animator>();
        programmerSkill = GetComponent<ProgrammerSkill>();
        isDead = false;
        extraDash = extraDashValue;
        extraJump = extraJumpValue;
        Physics.gravity = new Vector3(0, -gravityScale, 0);  //掉落重力
        isUseSkill = false;
        ProgrammerCanvas.SetActive(true);
    }
    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
    }
    void Update()
    {
        if (isDead == false)
        {
            if (isUseSkill == false)
            {
                Move();
                Jump();

                //DASH
                if (canDash == true)
                {
                    extraDash = extraDashValue;
                }
                if (Input.GetKeyDown(KeyCode.Keypad1) && extraDash > 0)
                {
                    StartCoroutine(Dash());
                    PlayerAni.SetTrigger("Dash");
                    extraDash--;
                }
                else if (Input.GetKeyDown(KeyCode.Keypad1) && extraDash == 0 && canDash == true && isGrounded == false)
                {
                    StartCoroutine(Dash());
                    PlayerAni.SetTrigger("Dash");
                    canDash = false;
                }

                //shoot
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    Shoot();
                    PlayerAni.SetTrigger("attack");
                }

                //掉落速度
                if (playerRigidbody.velocity.y < 0)
                {
                    playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }

                //Rotate
                horizontalInput = Input.GetAxis("HorizontalPlayer2");
                if (transform.localPosition.x != 0)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), 1f);
                }
                playerRigidbody.velocity = new Vector3(horizontalInput * moveSpeed, playerRigidbody.velocity.y, 0);
                if (horizontalInput > 0) { faceLeft = 1f; } else if (horizontalInput < 0) { faceLeft = -1f; }
                Vector3 movementDirection = new Vector3(horizontalInput * moveSpeed, 0, 0);

                //transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
                    SP.transform.rotation = Quaternion.RotateTowards(SP.transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (isUseSkill == false)
                {
                    isUseSkill = true;
                    programmerSkill.isUseSkill = true;
                    useSkillCanvas.SetActive(true);
                    PlayerAni.SetBool("useskill", true);
                }
                else if (isUseSkill == true)
                {
                    isUseSkill = false;
                    programmerSkill.isUseSkill = false;
                    useSkillCanvas.SetActive(false);
                    PlayerAni.SetBool("useskill", false);
                }
            }
        }
    }
    void Move()
    {
        if (Input.GetAxis("HorizontalPlayer2") > 0)
        {
            Run();
            PlayerAni.SetBool("onLeftWall", false);
        }
        else if (Input.GetAxis("HorizontalPlayer2") < 0)
        {
            Run();
            PlayerAni.SetBool("onRightWall", false);
        }
        else if (Input.anyKey != true && isGrounded == true)
        {
            Idle();
        }
    }

    void Run()
    {
        PlayerAni.SetBool("moving", true);
    }

    void Idle()
    {
        if (isGrounded == true && Input.anyKey != true)
            PlayerAni.SetBool("moving", false);
    }

    void Jump()
    {
        if (isGrounded == true) //回到地上重製跳躍次數
        {
            extraJump = extraJumpValue;
            isOnWall = false;
            PS.SetActive(false);
        }
        else if (isOnWall == true) //到牆上重製跳躍次數
        {
            extraJump = 0;
            isGrounded = false;
            PS.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJump > 0 && isGrounded == true) //Jump from the ground
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
            print("JUMP");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJump > 0) //double Jump OR Air Jump
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("DJump");
            print("DOUBLE JUMP");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJump == 0 && isGrounded == true) //確保跳躍時一定能跳起來 (正常狀況下不會執行)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJump == 0 && isOnWall == true) //WallJump
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
            print("WALL JUMP");
            isOnWall = false;
        }
    }

    //DASH
    IEnumerator Dash()
    {
        float startTime = Time.time;
        canDash = false;
        while (Time.time < startTime + dashTime)
        {
            playerRigidbody.AddForce(transform.forward * dashSpeed * faceLeft, ForceMode.VelocityChange);
            print("dash");
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    //Wall Check
    void WallCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.right, distantToWall + 0.1f) && !isGrounded)
        {
            isOnWall = true;
            PlayerAni.SetBool("onRightWall", true);
            print("isOnRIGHTWall");
        }
        else if (Physics.Raycast(transform.position, Vector3.left, distantToWall + 0.1f) && !isGrounded)
        {
            isOnWall = true;
            PlayerAni.SetBool("onLeftWall", true);
            print("isOnLEFTWall");
        }
        else if (isGrounded == true)
        {
            isOnWall = false;
        }
    }
    //Ground Check
    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distantToGround + 0.1f))
        {
            isGrounded = true;
            PlayerAni.SetBool("isgrounded", true);
            PlayerAni.SetBool("onRightWall", false);
            PlayerAni.SetBool("onLeftWall", false);
        }
        else
        {
            isGrounded = false;
            PlayerAni.SetBool("isgrounded", false);
        }
    }

    void Shoot()
    {
        /*if (PV.IsMine)
            PhotonNetwork.Instantiate("bullet", shootPoint.position, shootPoint.rotation);*/
    }
}
