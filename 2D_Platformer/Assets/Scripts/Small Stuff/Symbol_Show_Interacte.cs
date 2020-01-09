using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Show_Interacte : LevelInteracte
{
    [SerializeField]private List<SymbolNBT> symbolNBTs;

    private SymbolNBT symbolNBT = null;

    [System.Serializable]
    private class SymbolNBT
    {
        public Sprite sprite;
        public int NBT;
    }

    public override void InteractOn()
    {
        base.InteractOn();
        if (symbolNBT == null)
        {
            symbolNBT = symbolNBTs[Random.Range(0, symbolNBTs.Count)];
            GetComponent<SpriteRenderer>().sprite = symbolNBT.sprite;
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                foreach (var lI in item.GetComponents<LevelInteracte>())
                {
                    if (lI.NBT == symbolNBT.NBT)
                    {
                        lI.InteractOn();
                    }
                }
            }
        }
    }

    public override void InteractOff()
    {
        base.InteractOff();
        if (symbolNBT != null)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                foreach (var lI in item.GetComponents<LevelInteracte>())
                {
                    if (lI.NBT == symbolNBT.NBT)
                    {
                        lI.InteractOff();
                    }
                }
            }
            symbolNBT = null;
        }
    }
}
