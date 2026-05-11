using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    public GameObject artistCanvas;
    public float moveSpeed;
    private Rigidbody playerRigidbody;
    public float jumpForce;
    public float gravityScale;
    public float dashSpeed;
    public float dashTime;
    public float rotateSpeed;
    private bool canDash = true, isGrounded = false, isOnWall = false;
    public float distantToGround = 1f;
    public float distantToWall = 1f;
    private float horizontalInput, verticalInput;
    public int extraJump;
    public int extraJumpValue;
    [SerializeField] private GameObject body;
    [SerializeField] private float radius;

    private bool isWaiting;
    private float timer, DTimer;

    private InputMaster inputMaster;
    private Vector2 movement;

    private int extraDash;
    public int extraDashValue;
    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    public Image hpBar;
    public Animator PlayerAni;
    private AnimatorStateInfo AniInfo;
    private int hitCount = 0;
    public bool isDead;
    private bool bossFight, isHurt, BossDead;
    PhotonView PV;
    private PlayerManager playerManager;
    public GameObject myCamera, camTarget, HitboxCollider, TalkBox;
    private CameraController cameraController;
    private ArtistSkill skill;
    private InstantiateItems instantiateItems;
    private float UnrivaledTime = 0.7f;
    public GameObject PS;
    private int jumpInput;
    private bool isJump, isDash;

    private LayerMask Wall;
    [SerializeField] private float dieOnBoss;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        PlayerAni = GetComponentInChildren<Animator>();
        skill = GetComponent<ArtistSkill>();
        instantiateItems = GetComponent<InstantiateItems>();
        HitboxCollider.SetActive(false);
        isDead = false;
        BossDead = false;
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
        hitCount = 0;
        extraDash = extraDashValue;
        extraJump = extraJumpValue;
        artistCanvas.SetActive(true);
        jumpInput = 0;
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
            playerRigidbody.AddForce(0.5f * gravityScale * -Vector3.up);
        }
        else if (isDash)
        {
            playerRigidbody.AddForce(Vector3.zero);
        }
        else
        {
            playerRigidbody.AddForce(-Vector3.up * gravityScale);
        }
    }
    void Update()
    {
        if (!PV.IsMine)
            return;
        GroundCheck();
        WallCheck();
        movement = inputMaster.Player.Movement.ReadValue<Vector2>();
        hpBar.fillAmount = currentHealth / maxHealth;
        //moveInput = Input.GetAxis("Horizontal");        
        //playerRigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, playerRigidbody.velocity.y, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        bossFight = cameraController.isBossFight;
        //otherPlayerHpBar.fillAmount = programmer.currentHealth / maxHealth;
        if (!isDead)
        {
            if (isHurt)
            {
                UnrivaledTime -= Time.deltaTime;
                if (UnrivaledTime < 0f)
                {
                    isHurt = false;
                    UnrivaledTime = 0.8f;
                }
            }
            Move();
            Jump();
            Wait();
            //DASH
            if (canDash)
            {
                extraDash = extraDashValue;
            }
            
            AniInfo = PlayerAni.GetCurrentAnimatorStateInfo(0);
            if (!AniInfo.IsName("A idle1") && AniInfo.normalizedTime > 1.0f)
            {
                PlayerAni.SetInteger("attack", 0);
                hitCount = 0;
                HitboxCollider.SetActive(false);
            }
            //attack
            inputMaster.Player.Attack.performed +=_ => Attack();
            //Rotate
            horizontalInput = movement.x;
            if (bossFight)
            {
                verticalInput = movement.y;
            }
            else if (!bossFight)
            {
                if (transform.localPosition.x != 0)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), 1f);
                }
            }
            if (!isWaiting)
            {
                if (isDash)
                    return;
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
                Die();
            }
        }
        else if (isDead)
        {
            if (BossDead)
            {
                DTimer += Time.deltaTime;
                currentHealth += (maxHealth /dieOnBoss) * Time.deltaTime;
                if (Input.anyKey)
                {
                    currentHealth += 1f;
                    DTimer += 0.05f;
                }
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
            PlayerAni.SetBool("moving", true);
            PlayerAni.SetBool("onLeftWall", false);
        }
        else if (movement.x < 0)
        {
            PlayerAni.SetBool("moving", true);
            PlayerAni.SetBool("onRightWall", false);
        }
        else if (isGrounded == true)
        {
            PlayerAni.SetBool("moving", false);
            //Idle();
        }
    }
    void Jump()
    {
        if (isGrounded == true) //回到地上重製跳躍次數
        {
            extraJump = extraJumpValue;
            isOnWall = false;
            PS.SetActive(false);
            isJump = false;
            jumpInput = 0;
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
            //playerRigidbody.AddForce(body.transform.forward * dashSpeed, ForceMode.VelocityChange);
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
        if (!isOnWall)
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
    void Attack()
    {
        //PlayerAni.SetTrigger("attack");
        HitboxCollider.SetActive(true);
        if (AniInfo.normalizedTime > 0.1f && hitCount == 0)
        {
            hitCount = 1;
            PlayerAni.SetInteger("attack", 1);
            Debug.Log("c");           
        }
        else if (AniInfo.IsName("Base Layer.Armature|attack") && AniInfo.normalizedTime > 0.01f && hitCount == 1)
        {
            hitCount = 2;
            PlayerAni.SetInteger("attack", 2);
            Debug.Log("cc");
        }
        else if(AniInfo.IsName("Base Layer.Armature|attack2") && AniInfo.normalizedTime > 0.01f && hitCount == 2)
        {
            hitCount = 3;
            PlayerAni.SetInteger("attack", 3);
            Debug.Log("ccc");
        }
    }

    // this runs on the shooter's computer
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    // this runs on everyone's computer, but the !PV.IsMine CHECK makes it only run on the victim's computer
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
        hpBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("took damage: " + damage);
    }
    public void Die()
    {
        skill.ClearDraw();
        instantiateItems.cleanObject();
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
        playerManager.Die();
    }
}