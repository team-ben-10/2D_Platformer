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
    bool crouch = false;
    [HideInInspector] public bool onWall = false;
    public GameObject wallDetector;
    public bool isSecondPlayer;
    public GameObject second_Player;

    void Start()
    {
        wallJumpCount = maxWallJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSecondPlayer)
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
                animator.SetBool("Jump", true);
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
        }
    }

    [HideInInspector]public bool hasSpawnedSecondPlayer = false;

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
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * 75, controller.m_JumpForce * 1.25f));
            GetComponent<Rigidbody2D>().velocity =  new Vector2(transform.localScale.x/transform.localScale.y * 10f, 15f);
            wallJumpCount--;
            controller.m_AirControl = false;
            coroutineWallJump = StartCoroutine(waitForWalljump());
            animator.SetBool("Jump", true);
        }

        if (onWall && crouch && !isGravityChanged && !controller.m_Grounded)
        {
            GetComponent<Rigidbody2D>().gravityScale /= 12;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isGravityChanged = true;
        }
        if ((!onWall || !crouch || controller.m_Grounded) && isGravityChanged)
        {
            GetComponent<Rigidbody2D>().gravityScale *= 12;
            isGravityChanged = false;
        }
        controller.Move(horizontalMove * Time.deltaTime, crouch, jump);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    IEnumerator waitForWalljump()
    {
        yield return new WaitForSeconds(0.25f);
        controller.m_AirControl = true;
    }
}
