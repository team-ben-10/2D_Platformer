using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    int alphaNBT;
    void Start()
    {
        alphaNBT = GetComponent<AlphaNBTTag>().NBT;
    }
    bool hasDone = false;

    public virtual void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.StartsWith("Player"))
        {
            if (!hasDone)
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
                {
                    if (item.GetComponent<LevelInteracte>().NBT == alphaNBT)
                    {
                        item.GetComponent<LevelInteracte>().InteractOn();
                    }
                }
                hasDone = true;
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.StartsWith("Player"))
        {
            if (hasDone)
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
                {
                    if (item.GetComponent<LevelInteracte>().NBT == alphaNBT)
                    {
                        item.GetComponent<LevelInteracte>().InteractOff();
                    }
                }
                hasDone = false;
            }
        }
    }
}
