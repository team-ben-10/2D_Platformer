using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisInstaKillMushroomEffect : MushroomEffect
{
    int nbt;

    LevelInteracte interacte;
    private void Start()
    {
        if (GetComponent<AlphaNBTTag>() != null)
            nbt = GetComponent<AlphaNBTTag>().NBT;
        else
            nbt = -1;
        interacte = GetComponent<LevelInteracte>();
    }
    
    public override void OnDraw(GameObject player, float TimeLeft)
    {
        player.GetComponent<SpriteRenderer>().color = new Color(1, TimeLeft / Time, TimeLeft / Time);
    }

    public override void OnEnd(GameObject player, float timeLeft, int oldNBT)
    {
        if (oldNBT != nbt)
        {
            var objs = GameObject.FindGameObjectsWithTag("LeverInteract");
            foreach (var item in objs)
            {
                LevelInteracte i = item.GetComponent<LevelInteracte>();
                if (i.NBT == nbt)
                {
                    i.InteractOff();
                }
            }
        }
        if (timeLeft <= 0)
        {
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            player.GetComponent<PlayerStats>().Die();
        }
    }

    bool givenDarkness = false;

    public override void OnStart(GameObject player)
    {
        if(!givenDarkness)
            if(GameManager.instance.currentQuests.Exists((x)=>x.name == "A Mysterious Heart"))
            {
                GameManager.instance.CheckQuests("A Mysterious Heart");
                givenDarkness = true;
            }
        var objs = GameObject.FindGameObjectsWithTag("LeverInteract");
        if (objs.Length > 0)
        {
            foreach (var item in objs)
            {
                if (item != interacte)
                {
                    var lI = item.GetComponent<LevelInteracte>();
                    if (lI.NBT == nbt)
                    {
                        lI.InteractOn();
                    }
                }
            }
        }
    }
}
