using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisInstaKillMushroomEffect : MushroomEffect
{
    int nbt;
    private void Start()
    {
        if (GetComponent<AlphaNBTTag>() != null)
            nbt = GetComponent<AlphaNBTTag>().NBT;
        else
            nbt = -1;
    }
    
    public override void OnDraw(GameObject player, float TimeLeft)
    {
        player.GetComponent<SpriteRenderer>().color = new Color(1, TimeLeft / Time, TimeLeft / Time);
    }

    public override void OnEnd(GameObject player, float timeLeft, int oldNBT)
    {
        if (oldNBT != nbt)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (item.GetComponent<LevelInteracte>().NBT == nbt)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
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
                GameManager.instance.CheckQuests("A Mysterious Heart", 5);
                givenDarkness = true;
            }
        if (GameObject.FindGameObjectsWithTag("LeverInteract").Length > 0)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                Debug.Log(item.name);
                if (item != GetComponent<LevelInteracte>())
                {
                    if (item.GetComponent<LevelInteracte>().NBT == nbt)
                    {
                        item.GetComponent<LevelInteracte>().InteractOn();
                    }
                }
            }
        }
    }
}
