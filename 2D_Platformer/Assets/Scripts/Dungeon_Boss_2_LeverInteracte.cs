using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Boss_2_LeverInteracte : LevelInteracte
{
    public int nbt;

    private void Start()
    {
        NBT = nbt;
    }

    public override void InteractOff()
    {

    }

    public override void InteractOn()
    {
        if(NBT == 120)
        {
            Debug.Log("activate left arm");
            GetComponent<Enemy_Dungeon_2_Boss>().leftArmCol.isTrigger = true;
            GetComponent<Enemy_Dungeon_2_Boss>().rightArmCol.isTrigger = false;
            GetComponent<Enemy_Dungeon_2_Boss>().headCol.isTrigger = false;
        }
        if (NBT == 110)
        {
            Debug.Log("activate right arm");
            GetComponent<Enemy_Dungeon_2_Boss>().leftArmCol.isTrigger = false;
            GetComponent<Enemy_Dungeon_2_Boss>().rightArmCol.isTrigger = true;
            GetComponent<Enemy_Dungeon_2_Boss>().headCol.isTrigger = false;
        }
        if (NBT == 100)
        {
            Debug.Log("activate head");
            GetComponent<Enemy_Dungeon_2_Boss>().leftArmCol.isTrigger = false;
            GetComponent<Enemy_Dungeon_2_Boss>().rightArmCol.isTrigger = false;
            GetComponent<Enemy_Dungeon_2_Boss>().headCol.isTrigger = true;
        }
    }
}
