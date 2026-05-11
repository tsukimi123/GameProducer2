using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class PlayerController2 : MonoBehaviourPunCallbacks, IDamageable
{
    public GameObject ProgrammerCanvas;
    public float moveSpeed;
    private Rigidbody playerRigidbody;
    public float jumpForce;
    public float gravityScale;
    public float fallMultiplier = 2.5f;
    public float dashSpeed;
    public float dashTime;
    public float rotateSpeed;
    [SerializeField] private float radius;
    [SerializeField] private GameObject body;

    private bool canDash = true, isGrounded = false, isOnWall = false;
    public float distantToGround = 1f;
    public float distantToWall = 1f;
    private float horizontalInput, verticalInput;
    private int extraJump;
    public int extraJumpValue;

    private int extraDash;
    public int extraDashValue;

    private Vector3 mouse;
    private bool isWaiting;
    private float timer, DTimer;

    private InputMaster inputMaster;
    private Vector2 movement;

    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    public Image hpBar;
    public Transform shootPoint;
    public GameObject bulletPrefab;

    Animator PlayerAni;

    PhotonView PV;
    private PlayerManager2 playerManager2;
    private CameraController cameraController;
    public bool isDead;
    private bool bossFight, isHurt, BossDead;
    [SerializeField] private float UnrivaledTime = 0.7f;
    public GameObject switchSkillCanvas, useSkillCanvas;
    public GameObject myCamera, camTarget;
    public GameObject TalkBox;
    public GameObject PS;
    private int jumpInput;
    private bool isJump, isDash;
    private ProgrammerSkill skill;
    private Mouse mouseScript;
    private LayerMask Wall;
    [SerializeField] private float dieOnBoss;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager2 = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager2>();
        PlayerAni = GetComponentInChildren<Animator>();
        cameraController = myCamera.GetComponent<CameraController>();
        if (PV.IsMine)
        {
            inputMaster = new InputMaster();
            inputMaster.Enable();
        }
    }
    void Start()
    {
        if (!PV.IsMine)
            return;
        extraDash = extraDashValue;
        extraJump = extraJumpValue;
        jumpInput = 0;
        ProgrammerCanvas.SetActive(true);
        mouseScript = GameObject.Find("Mouse").GetComponent<Mouse>();
        skill = GetComponent<ProgrammerSkill>();
        Wall = 1 << (LayerMask.NameToLayer("Wall"));
        inputMaster.Player.Dash.performed += _ =>
        {
            if (extraDash > 0)
            {
                PlayerAni.SetTrigger("Dash");
                StartCoroutine(Dash());
                timer = 1f;
                extraDash--;
            }
            else if (extraDash == 0 && canDash == true && isGrounded == false)
            {
                PlayerAni.SetTrigger("Dash");
                StartCoroutine(Dash());
                timer = 1f;
                canDash = false;
            }
        };
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        if (isOnWall)
        {
            playerRigidbody.AddForce(0.5f * gravityScale * Vector3.down);
        }
        else if (isDash)
        {
            playerRigidbody.AddForce(Vector3.zero);
        }
        else
        {
            playerRigidbody.AddForce(Vector3.down * gravityScale);
        }
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;
        GroundCheck();
        WallCheck();
        hpBar.fillAmount = currentHealth / maxHealth;
        movement = inputMaster.Player.Movement.ReadValue<Vector2>();
        bossFight = cameraController.isBossFight;
        if (isDead == false)
        {
            if (isHurt == true)
            {
                UnrivaledTime -= Time.deltaTime;
                if (UnrivaledTime < 0f)
                {
                    isHurt = false;
                    UnrivaledTime = 1.5f;
                }
            }
            Move();
            Jump();
            Wait();
            //DASH
            if (canDash == true)
            {
                extraDash = extraDashValue;
            }
            //shoot
            if (mouseScript.useJoyStick == true)
            {
                mouse = Camera.main.ScreenToWorldPoint(new Vector3(mouseScript.IconPosition.x, mouseScript.IconPosition.y, -Camera.main.transform.position.z));
            }
            else if (mouseScript.useJoyStick == false)
            {
                mouse = skill.cameraTransform;
            }
            shootPoint.LookAt(mouse);
            inputMaster.Player.Attack.performed += _ => Shoot();
            //Rotate
            horizontalInput = movement.x;
            if (bossFight == true)
            {
                verticalInput = movement.y;
            }
            else if (bossFight == false)
            {
                if (transform.localPosition.x != 0)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), 1f);
                }
            }
            if (isWaiting == false)
            {
                playerRigidbody.velocity = new Vector3(horizontalInput * moveSpeed, playerRigidbody.velocity.y, verticalInput * moveSpeed);
                Vector3 movementDirection = new Vector3(horizontalInput * moveSpeed, 0, verticalInput * moveSpeed);
                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
                }
            }
            //Die
            if (transform.position.y < -50f || Input.GetKeyDown(KeyCode.K))
            {
                Die(); //Die if you fall out the world
            }
        }
        else if (isDead)
        {
            if (BossDead)
            {
                DTimer += Time.deltaTime;
                currentHealth += (maxHealth / dieOnBoss) * Time.deltaTime;
                if (DTimer > dieOnBoss)
                {
                    isDead = false;
                    BossDead = false;
                    PlayerAni.SetBool("Dead", false);
                    currentHealth = maxHealth;
                    DTimer = 0f;
                }
            }
        }
    }

    void Move()
    {
        if (movement.x > 0)
        {
            Run();
            PlayerAni.SetBool("onLeftWall", false);
        }
        else if (movement.x < 0)
        {
            Run();
            PlayerAni.SetBool("onRightWall", false);
        }
        else if (isGrounded == true)
        {
            PlayerAni.SetBool("moving", false);
            //Idle();
        }
    }

    void Run()
    {
        PlayerAni.SetBool("moving", true);
    }

    void Jump()
    {
        if (isGrounded == true) //回到地上重製跳躍次數
        {
            extraJump = extraJumpValue;
            isOnWall = false;
            PS.SetActive(false);
            jumpInput = 0;
            isJump = false;
        }
        else if (isOnWall == true) //到牆上重製跳躍次數
        {
            extraJump = 0;
            isGrounded = false;
            PS.SetActive(false);
        }
        inputMaster.Player.Jump.performed += _ =>
        {
            jumpInput += 1;
            if (extraJump > 0 && isGrounded == true) //Jump from the ground
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
                playerRigidbody.velocity += Vector3.up * jumpForce;
                extraJump--;
                isJump = true;
                PS.SetActive(true);
                PlayerAni.SetTrigger("Jump");
                print("JUMP");
            }
            else if (extraJump > 0 || jumpInput >= 1 && extraJump > 0 && isJump == true) //double Jump OR Air Jump
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
                playerRigidbody.velocity = Vector3.up * jumpForce;
                extraJump--;
                PS.SetActive(true);
                PlayerAni.SetTrigger("DJump");
                print("DOUBLE JUMP");
            }
            else if (extraJump == 0 && isOnWall == true) //WallJump
            {
                Vector3 facingDir = body.transform.forward;
                if (facingDir.x < 0)
                {
                    playerRigidbody.velocity = new Vector3(0.7f, 0.9f, 0) * jumpForce;
                    body.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                }
                else
                {
                    playerRigidbody.velocity = new Vector3(-0.7f, 0.9f, 0) * jumpForce;
                    body.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                }
                extraJump = -1;
                PS.SetActive(true);
                PlayerAni.SetTrigger("Jump");
                print("WALL JUMP");
                isOnWall = false;
            }
            else if (extraJump == 0 && isGrounded == true) //確保跳躍時一定能跳起來 (正常狀況下不會執行)
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
                playerRigidbody.velocity += Vector3.up * jumpForce;
                PS.SetActive(true);
                PlayerAni.SetTrigger("Jump");
            }
        };
    }
    private void Wait()
    {
        if (isWaiting == true)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 1.25f || isGrounded == true)
        {
            isWaiting = false;
            timer = 0;
        }
    }
    //DASH
    IEnumerator Dash()
    {
        float startTime = Time.time;
        canDash = false;
        isDash = true;
        while (Time.time < startTime + dashTime)
        {
            playerRigidbody.velocity = dashSpeed * body.transform.forward;
            print("dash");
            yield return 0;
        }
        isDash = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    //Wall Check
    void WallCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.right, distantToWall + 0.1f, Wall) && !isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.right * (distantToWall + 0.1f), Color.green);
            isOnWall = true;
            isWaiting = true;
            PlayerAni.SetBool("onRightWall", true);
            print("isOnRIGHTWall");
        }
        else if (Physics.Raycast(transform.position, Vector3.left, distantToWall + 0.1f, Wall) && !isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.left * (distantToWall + 0.1f), Color.green);
            isOnWall = true;
            isWaiting = true;
            PlayerAni.SetBool("onLeftWall", true);
            print("isOnLEFTWall");
        }
        else
        {
            isOnWall = false;
        }
        if (isOnWall == false)
        {
            PlayerAni.SetBool("onRightWall", false);
            PlayerAni.SetBool("onLeftWall", false);
        }
    }
    //Ground Check
    void GroundCheck()
    {
        Debug.DrawRay(transform.position + Vector3.left * radius, Vector3.down * (distantToGround + 0.5f), Color.green);
        Debug.DrawRay(transform.position + Vector3.right * radius, Vector3.down * (distantToGround + 0.5f), Color.green);
        if (Physics.Raycast(transform.position + Vector3.left * radius, Vector3.down, distantToGround + 0.5f) || Physics.Raycast(transform.position + Vector3.right * radius, Vector3.down, distantToGround + 0.5f))
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
        PlayerAni.SetTrigger("attack");
            PhotonNetwork.Instantiate("bullet", shootPoint.position, shootPoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;
        if (isHurt == true)
            return;
        isHurt = true;
        PlayerAni.SetTrigger("behurt");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("took damage: " + damage);
    }
    public void Die()
    {
        isDead = true;
        PlayerAni.SetBool("Dead", true);
        if (bossFight)
        {
            BossDead = true;
        }
        else if (!bossFight)
        {
            currentHealth -= 100f;
            hpBar.fillAmount = currentHealth / maxHealth;
            inputMaster.Disable();
            StartCoroutine(ReallyDie());
        }
    }
    IEnumerator ReallyDie()
    {
        yield return new WaitForSeconds(2.5f);
        PhotonNetwork.Destroy(this.gameObject);
        playerManager2.Die();
    }
}