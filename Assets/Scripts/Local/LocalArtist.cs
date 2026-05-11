using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalArtist : MonoBehaviour
{
    public GameObject artistCanvas;
    public float moveSpeed;
    public Rigidbody playerRigidbody;
    public float jumpForce;
    public float gravityScale;
    public float fallMultiplier = 2.5f;
    public float dashSpeed;
    public float dashTime;
    public float rotateSpeed;

    private float moveInput;

    private bool canDash = true;
    private bool isGrounded = false;
    private bool isOnWall = false;
    public float distantToGround = 1f;
    public float distantToWall = 1f;
    private float horizontalInput;
    public int extraJump;
    public int extraJumpValue;

    private int extraDash;
    public int extraDashValue;
    private const float maxHealth = 100f;
    public Animator PlayerAni;
    private AnimatorStateInfo AniInfo;
    private int hitCount = 0;
    public GameObject HitboxCollider;
    [SerializeField] private GameObject body;
    private ArtistSkill skill;
    private InstantiateItems instantiateItems;
    public bool isDead, bossFight;
    private float faceLeft = 1;
    public GameObject PS;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        PlayerAni = GetComponentInChildren<Animator>();
        skill = GetComponent<ArtistSkill>();
        instantiateItems = GetComponent<InstantiateItems>();
        HitboxCollider.SetActive(false);
        isDead = false;
        hitCount = 0;
        extraDash = extraDashValue;
        extraJump = extraJumpValue;
        Physics.gravity = new Vector3(0, -gravityScale, 0);  //掉落重力
        artistCanvas.SetActive(true);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
    }
    private void Update()
    {
        if (isDead == false)
        {
            Move();
            Jump();
            //DASH
            if (canDash == true)
            {
                extraDash = extraDashValue;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && extraDash > 0)
            {
                StartCoroutine(Dash());
                PlayerAni.SetTrigger("Dash");
                extraDash--;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && extraDash == 0 && canDash == true && isGrounded == false)
            {
                StartCoroutine(Dash());
                PlayerAni.SetTrigger("Dash");
                canDash = false;
            }
            AniInfo = PlayerAni.GetCurrentAnimatorStateInfo(0);
            if (!AniInfo.IsName("A idle1") && AniInfo.normalizedTime > 1.0f)
            {
                PlayerAni.SetInteger("attack", 0);
                hitCount = 0;
                HitboxCollider.SetActive(false);
            }
            //attack
            if (Input.GetKeyDown(KeyCode.F))
            {
                Attack();
            }
            //掉落速度
            if (playerRigidbody.velocity.y < 0)
            {
                playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            //Rotate
            horizontalInput = Input.GetAxis("HorizontalPlayer1");
            if (transform.localPosition.x != 0)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, transform.localPosition.z), 1f);
            }
            playerRigidbody.velocity = new Vector3(horizontalInput * moveSpeed, playerRigidbody.velocity.y, 0);
            if (horizontalInput > 0) { faceLeft = 1f; } else if (horizontalInput < 0) { faceLeft = -1f; }
            Vector3 movementDirection = new Vector3(horizontalInput * moveSpeed, 0, 0);
            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerAni.SetBool("useskill", true);
            }
        }
    }
    void Move()
    {
        if (Input.GetAxis("HorizontalPlayer1") > 0)
        {
            Run();
            PlayerAni.SetBool("onLeftWall", false);
        }
        else if (Input.GetAxis("HorizontalPlayer1") < 0)
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
        /*if (isGrounded == true)
            PlayerAni.SetFloat("Speed", 1, 0.1f, Time.deltaTime);*/
    }

    void Idle()
    {
        if (isGrounded == true && Input.anyKey != true)
            PlayerAni.SetBool("moving", false);
        /*if (isGrounded == true)
            PlayerAni.SetFloat("Speed", 0, 0.1f, Time.deltaTime);*/
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
        if (Input.GetButtonDown("Jump") && extraJump > 0 && isGrounded == true) //Jump from the ground
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
        }
        else if (Input.GetButtonDown("Jump") && extraJump > 0) //double Jump OR Air Jump
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("DJump");
        }
        else if (Input.GetButtonDown("Jump") && extraJump == 0 && isOnWall == true) //WallJump
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            extraJump--;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
            isOnWall = false;
        }
        else if (Input.GetButtonDown("Jump") && extraJump == 0 && isGrounded == true) //確保跳躍時一定能跳起來 (正常狀況下不會執行)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.velocity += Vector3.up * jumpForce;
            PS.SetActive(true);
            PlayerAni.SetTrigger("Jump");
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
            Debug.DrawRay(transform.position, Vector3.right * (distantToWall + 0.1f), Color.green);
            isOnWall = true;
            PlayerAni.SetBool("onRightWall", true);
        }
        else if (Physics.Raycast(transform.position, Vector3.left, distantToWall + 0.1f) && !isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.left * (distantToWall + 0.1f), Color.green);
            isOnWall = true;
            PlayerAni.SetBool("onLeftWall", true);
        }
        else if (isGrounded == true)
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
        if (Physics.Raycast(transform.position, Vector3.down, distantToGround + 0.5f))
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
        if (PlayerAni.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f && hitCount == 0)
        {
            hitCount = 1;
            PlayerAni.SetInteger("attack", 1);
            Debug.Log("c");
        }
        else if (PlayerAni.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|attack") && PlayerAni.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f && hitCount == 1)
        {
            hitCount = 2;
            PlayerAni.SetInteger("attack", 2);
            Debug.Log("cc");
        }
        else if (PlayerAni.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|attack2") && PlayerAni.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f && hitCount == 2)
        {
            hitCount = 3;
            PlayerAni.SetInteger("attack", 3);
            Debug.Log("ccc");
        }
    }
}
