using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_MushroomEffect : MushroomEffect
{
    
    public override void OnStart(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().gravityScale *= 0.5f;
        player.transform.localScale /= 2;
    }

    public override void OnEnd(GameObject player,float timeLeft, int oldNBT)
    {
        player.GetComponent<Rigidbody2D>().gravityScale *= 2;
        player.transform.localScale *= 2;
    }

}
