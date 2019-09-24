using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
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
        if (transform.tag == "Player")
        {
            if (!controller.m_Grounded && Input.GetButtonDown("Jump") && !GetComponent<PlayerMovement>().onWall)
            {
                if (canUse)
                {
                    rb.velocity = Vector2.zero;
                    var effect = Instantiate(objsToSpawn[0], transform.position, Quaternion.identity);
                    effect.transform.eulerAngles = new Vector3(-90, controller.m_FacingRight ? 180 : 0, 0);
                    Destroy(effect, 2);
                    rb.AddForce(transform.right*5000*(controller.m_FacingRight?1:-1));
                    canUse = false;
                }
            }
        }
        else if (transform.tag == "Player_2")
        {
            if (!controller.m_Grounded && Input.GetButtonDown("Jump_Player_2") && !GetComponent<PlayerMovement>().onWall)
            {
                if (canUse)
                {
                    rb.velocity = Vector2.zero;
                    var effect = Instantiate(objsToSpawn[0], transform.position, Quaternion.identity);
                    effect.transform.eulerAngles = new Vector3(-90, controller.m_FacingRight ? 180 : 0, 0);
                    Destroy(effect, 2);
                    rb.AddForce(transform.right * 5000 * (controller.m_FacingRight ? 1 : -1));
                    canUse = false;
                }
            }
        }
        if (controller.m_Grounded || GetComponent<PlayerMovement>().onWall)
        {
            canUse = true;
        }
    }
}
