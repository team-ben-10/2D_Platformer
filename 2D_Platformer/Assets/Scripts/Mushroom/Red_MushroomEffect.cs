using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_MushroomEffect : MushroomEffect
{
    float standardScale;
    public override void OnStart(GameObject player)
    {
        standardScale = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    public override void OnDraw(GameObject player, float TimeLeft)
    {
        if(!player.GetComponent<PlayerMovement>().controller.m_Grounded)
        {
            var preset = InputManager.instance.GetPreset(player.transform.tag);
            if (InputManager.instance.GetButton("Jump",preset))
            {
                player.GetComponent<Rigidbody2D>().gravityScale = standardScale / 5;
            }
            else
            {
                player.GetComponent<Rigidbody2D>().gravityScale = standardScale;
            }
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = standardScale;

        }
    }

    public override void OnEnd(GameObject player,float timeLeft, int oldNBT)
    {
        player.GetComponent<Rigidbody2D>().gravityScale = standardScale;
    }
}
