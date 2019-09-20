using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    bool unloced = false;
    public Dictionary<int, string> dic = new Dictionary<int, string>();
    private void Start()
    {
        dic.Add(200, "Alien");
        dic.Add(255, "Standard");
        dic.Add(245, "Stone");
        dic.Add(240, "Corrupted");
        dic.Add(235, "Knight");
        unloced = dic[GetComponent<AlphaNBTTag>().NBT] == "Standard" ? true : GameManager.instance.IsUnloced(dic[GetComponent<AlphaNBTTag>().NBT]);
        if (unloced)
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = GameManager.instance.GetCharacter(dic[GetComponent<AlphaNBTTag>().NBT]).controller;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (unloced)
            {
                GameManager.instance.SetCurrentPlayer(GameManager.instance.GetCharacter(dic[GetComponent<AlphaNBTTag>().NBT]), (collision.tag == "Player_2"));
                GameManager.instance.UpdatePlayer();
            }
        }
    }
}
