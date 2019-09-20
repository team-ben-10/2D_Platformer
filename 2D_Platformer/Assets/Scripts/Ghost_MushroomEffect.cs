using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_MushroomEffect : MushroomEffect
{

    public override void OnStart(GameObject player)
    {
        player.layer = LayerMask.NameToLayer("PlayerGhost");
    }

    public override void OnEnd(GameObject player,float timeLeft, int oldNBT)
    {
        player.layer = LayerMask.NameToLayer("Player");
    }

}
