using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Opossum : Enemy
{
    bool alive = true;
    protected override void onTouch(GameObject player, Collider2D col)
    {
        if (player.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            Destroy(this.gameObject);
            var Kinderriegel = col.transform.GetComponent<Rigidbody2D>().velocity;
            col.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(Kinderriegel.x, 10);
            alive = false;
        }
        else if(alive)
        {
            player.GetComponent<PlayerStats>().Die();

        }
    }
}
