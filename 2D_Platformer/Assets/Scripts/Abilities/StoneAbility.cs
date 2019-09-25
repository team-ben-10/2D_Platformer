using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAbility : Ability
{
    
    Vector2 standardGravity;

    void Start()
    {
        standardGravity = Physics2D.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Crouch" + (transform.tag == "Player" ? "" : ("_" + transform.tag))))
        {
            if (Time.timeScale > 0.5f)
            {
                Time.timeScale -= 0.025f;
            }
        }
        if (!Input.GetButton("Crouch" + (transform.tag == "Player" ? "" : ("_" + transform.tag))))
        {
            if (Time.timeScale < 1f)
                Time.timeScale += 0.025f;
        }
        Time.fixedDeltaTime = Time.timeScale * .02f;
        Physics2D.gravity = standardGravity * Time.timeScale;
    }
}
