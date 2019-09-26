using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    public int maxWallJumpCount = 3;
    [HideInInspector]public float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    [HideInInspector] public bool crouch = false;
    [HideInInspector] public bool onWall = false;
    public GameObject wallDetector;
    public bool isNotMain;
    public GameObject otherPlayer;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallJumpCount = maxWallJumpCount;
    }

    [HideInInspector]public int PlayerSpawnIndex = 2;
    public int MaxPlayerAtATime = 4;

    // Update is called once per frame
    void Update()
    {
        #region Old 2 Player 
        /*if (isSecondPlayer)
        {
            var colliders = Physics2D.OverlapCircleAll(wallDetector.transform.position, .1f, controller.m_WhatIsGround);
            foreach (var item in colliders)
            {
                if (item.gameObject != gameObject)
                {
                    onWall = true;
                }
            }
            
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            if (colliders.Length <= 0)
            {
                onWall = false;
            }
            horizontalMove = Input.GetAxisRaw("Horizontal_Player_2") * runSpeed;
            if (Input.GetAxisRaw("Horizontal_Player_2") <= 0.35 && Input.GetAxisRaw("Horizontal_Player_2") >= -0.35)
            {
                horizontalMove = 0;
            }
            if (Input.GetButtonDown("Jump_Player_2"))
            {
                jump = true;
                animator.SetBool("Jump", true);
            }
            if (Input.GetButtonDown("Crouch_Player_2"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch_Player_2"))
            {
                crouch = false;
            }
            animator.SetBool("Crouch", crouch);
        }
        else
        {
            if (Input.GetButtonDown("Spawn_Second_Player") && !hasSpawnedSecondPlayer)
            {
                Debug.Log("Spawn 2nd Player");
                GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Add(Instantiate(second_Player, transform.position + new Vector3(0, 0.5f), Quaternion.identity).transform);
                GameManager.instance.UpdatePlayer();
                hasSpawnedSecondPlayer = true;
            }
            var colliders = Physics2D.OverlapCircleAll(wallDetector.transform.position, .1f, controller.m_WhatIsGround);
            foreach (var item in colliders)
            {
                if (item.gameObject != gameObject)
                {
                    onWall = true;
                }
            }
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            if (colliders.Length <= 0)
            {
                onWall = false;
            }
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetAxisRaw("Horizontal") <= 0.35 && Input.GetAxisRaw("Horizontal") >= -0.35)
            {
                horizontalMove = 0;
            }
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }
            animator.SetBool("Crouch", crouch);
        }*/

        #endregion
        if (Input.GetButtonDown("Player_Join") && !isNotMain && PlayerSpawnIndex < MaxPlayerAtATime)
        {
            Debug.Log("Spawn Player");
            var playerTransform = Instantiate(otherPlayer, transform.position + new Vector3(0, 0.5f), Quaternion.identity).transform;
            playerTransform.tag = "Player_" + PlayerSpawnIndex;
            //playerTransform.gameObject.layer = LayerMask.NameToLayer("Player_" + PlayerSpawnIndex);
            playerTransform.GetComponent<PlayerMovement>().isNotMain = true;
            GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Add(playerTransform);
            if(PlayerSpawnIndex-2 == 0)
                GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Add(transform);
            GameManager.instance.UpdatePlayer();
            //hasSpawnedSecondPlayer = true;
            PlayerSpawnIndex++;
        }

        var colliders = Physics2D.OverlapCircleAll(wallDetector.transform.position, .1f, controller.m_WhatIsGround);
        foreach (var item in colliders)
        {
            if (item.gameObject != gameObject)
            {
                onWall = true;
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (colliders.Length <= 0)
        {
            onWall = false;
        }
        horizontalMove = Input.GetAxisRaw("Horizontal" + (transform.tag != "Player"?"_" + transform.tag:"")) * runSpeed;
        if (Input.GetAxisRaw("Horizontal" + (transform.tag != "Player" ? "_" + transform.tag : "")) <= 0.35 && Input.GetAxisRaw("Horizontal"+(transform.tag != "Player" ? "_" + transform.tag : "")) >= -0.35)
        {
            horizontalMove = 0;
        }
        if (Input.GetButtonDown("Jump" + (transform.tag != "Player" ? "_" + transform.tag : "")))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        if (Input.GetButtonDown("Crouch" + (transform.tag != "Player" ? "_" + transform.tag : "")))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch" + (transform.tag != "Player" ? "_" + transform.tag : "")))
        {
            crouch = false;
        }
        animator.SetBool("Crouch", crouch);
    }

    //[HideInInspector]public bool hasSpawnedSecondPlayer = false;

    bool isGravityChanged = false;
    bool usedWallJump = false;
    int wallJumpCount;
    
    Coroutine coroutineWallJump;

    private void FixedUpdate()
    {
        if (controller.m_Grounded)
        {
            wallJumpCount = maxWallJumpCount;
            controller.m_AirControl = true;
            if (coroutineWallJump != null)
            {
                StopCoroutine(coroutineWallJump);
            }
        }
        if (onWall && jump && !controller.m_Grounded && wallJumpCount > 0)
        {
            controller.Flip();
            //rb.AddForce(new Vector2(transform.localScale.x * 75, controller.m_JumpForce * 1.25f));
            rb.velocity =  new Vector2(transform.localScale.x/transform.localScale.y * 10f, 15f);
            wallJumpCount--;
            controller.m_AirControl = false;
            coroutineWallJump = StartCoroutine(waitForWalljump());
            animator.SetBool("Jump", true);
            wallStuckTime = 0;
        }

        if (onWall && crouch && !isGravityChanged && !controller.m_Grounded && wallStuckTime <= 2)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            isGravityChanged = true;
        }
        if(onWall && crouch && !controller.m_Grounded && wallStuckTime <= 2)
        {
            wallStuckTime += Time.fixedDeltaTime;
        }
        if (((!onWall || !crouch || controller.m_Grounded) && isGravityChanged)||wallStuckTime > 2)
        {
            rb.gravityScale = 3;
            isGravityChanged = false;
        }
        if (controller.m_Grounded)
        {
            wallStuckTime = 0;
        }

        controller.Move(horizontalMove * Time.deltaTime, crouch, jump);
        if(jump)
            animator.SetBool("Jump", true);
        if (jump)
            jump = false;
    }

    float wallStuckTime = 0;

    public void OnLanding()
    {
        if(rb.velocity.y <= 0)
        animator.SetBool("Jump", false);
    }

    IEnumerator waitForWalljump()
    {
        yield return new WaitForSeconds(0.25f);
        controller.m_AirControl = true;
    }
}
