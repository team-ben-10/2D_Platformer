using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelup_MushroomEffect : MushroomEffect
{
    public override void OnDraw(GameObject player, float TimeLeft)
    {
        if (Input.GetButtonDown("Jump") && !player.GetComponent<PlayerMovement>().isSecondPlayer)
        {
            player.GetComponent<Rigidbody2D>().gravityScale *= -1;
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y * -1, player.transform.localScale.z);
        }
        if (Input.GetButtonDown("Jump_Player_2") && player.GetComponent<PlayerMovement>().isSecondPlayer)
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
