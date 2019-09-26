using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbAbility : Ability
{
    CharacterController2D controller;
    Rigidbody2D rb;
    PlayerMovement player;
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    float climbWallTime = 1;

    private void Update()
    {
        if (player.onWall&&climbWallTime > 0 && player.crouch)
        {
            climbWallTime -= Time.deltaTime;
            rb.velocity = Vector2.up;
        }
        if (controller.m_Grounded)
        {
            climbWallTime = 1;
        }
    }
}
