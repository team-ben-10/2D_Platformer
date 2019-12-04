using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAbility : Ability
{
    
    Vector2 standardGravity;

    InputManager.KeyPreset preset;

    void Start()
    {
        standardGravity = Physics2D.gravity;
        preset = InputManager.instance.GetPreset(transform.tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.GetButton("Crouch",preset))
        {
            if (Time.timeScale > 0.5f)
            {
                Time.timeScale -= 0.025f;
            }
        }
        if (!InputManager.instance.GetButton("Crouch", preset))
        {
            if (Time.timeScale < 1f)
                Time.timeScale += 0.025f;
        }
        Time.fixedDeltaTime = Time.timeScale * .02f;
        Physics2D.gravity = standardGravity * Time.timeScale;
    }
}
