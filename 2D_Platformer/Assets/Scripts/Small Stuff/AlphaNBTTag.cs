using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaNBTTag : MonoBehaviour
{
    public int NBT { private set; get; }
    public void setNBT(int nbt)
    {
        NBT = nbt;
    }
}
