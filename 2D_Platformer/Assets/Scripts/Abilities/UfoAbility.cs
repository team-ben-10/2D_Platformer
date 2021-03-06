﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoAbility : Ability
{

    InputManager.KeyPreset preset;
    private void Start()
    {
        preset = InputManager.instance.GetPreset(transform.tag);
    }


    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PlayerStats>().mushroomEffect != null)
            return;
        if (InputManager.instance.GetButton("Jump",preset) && !gameObject.GetComponent<CharacterController2D>().m_Grounded && !gameObject.GetComponent<PlayerMovement>().onWall && GetComponent<SpriteRenderer>().sprite == sprite && GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            Vector3 vel = gameObject.GetComponent<Rigidbody2D>().velocity;
            vel = new Vector3(vel.x, Mathf.Max(vel.y, -1), vel.z);
            gameObject.GetComponent<Rigidbody2D>().velocity = vel;
        }
        /*if (!Input.GetButton("Jump_Player_2") || gameObject.GetComponent<CharacterController2D>().m_Grounded || gameObject.GetComponent<PlayerMovement>().onWall || GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }*/
    }
}
