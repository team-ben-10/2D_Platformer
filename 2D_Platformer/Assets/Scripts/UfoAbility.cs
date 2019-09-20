using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoAbility : Ability
{
   
   
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PlayerStats>().mushroomEffect != null)
            return;
        if (transform.tag == "Player")
        {
            if (Input.GetButton("Jump") && !gameObject.GetComponent<CharacterController2D>().m_Grounded && !gameObject.GetComponent<PlayerMovement>().onWall && GetComponent<SpriteRenderer>().sprite == sprite && GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            }
            if (!Input.GetButton("Jump") || gameObject.GetComponent<CharacterController2D>().m_Grounded || gameObject.GetComponent<PlayerMovement>().onWall || GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
            }
        }
        else if (transform.tag == "Player_2")
        {
            if (Input.GetButton("Jump_Player_2") && !gameObject.GetComponent<CharacterController2D>().m_Grounded && !gameObject.GetComponent<PlayerMovement>().onWall && GetComponent<SpriteRenderer>().sprite == sprite && GetComponent<Rigidbody2D>().velocity.y <= 0)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            }
            if (!Input.GetButton("Jump_Player_2") || gameObject.GetComponent<CharacterController2D>().m_Grounded || gameObject.GetComponent<PlayerMovement>().onWall || GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
            }
        }
    }
}
