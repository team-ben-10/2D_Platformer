using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Ball : Enemy
{
    protected override void onTouch(GameObject player, Collider2D col)
    {
        player.GetComponent<PlayerStats>().Die();
    }
}
