using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelup_MushroomEffect : MushroomEffect
{
    public override void OnDraw(GameObject player, float TimeLeft)
    {
        var preset = InputManager.instance.GetPreset(player.transform.tag);
        if (InputManager.instance.GetButtonDown("Jump",preset))
        {
            player.GetComponent<Rigidbody2D>().gravityScale *= -1;
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y * -1, player.transform.localScale.z);
        }
    }

    public override void OnEnd(GameObject player, float timeLeft, int oldNBT)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        if(rb.gravityScale <= 0)
        {
            rb.gravityScale *= -1;
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y * -1, player.transform.localScale.z);
        }
    }
}
