using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : Ability
{
    CharacterController2D controller;
    Rigidbody2D rb;
    bool canUse = false;
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!controller.m_Grounded && Input.GetButtonDown("Jump" + (transform.tag == "Player" ? "" : ("_" + transform.tag))) && !GetComponent<PlayerMovement>().onWall)
        {
            if (canUse)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, controller.m_JumpForce));
                canUse = false;
            }
        }
        if (controller.m_Grounded || GetComponent<PlayerMovement>().onWall)
        {
            canUse = true;
        }
    }
}
