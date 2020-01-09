using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_Selection_Prefab : MonoBehaviour
{
    [HideInInspector] public CreatorManager.ObjClick gb;

    public void OnInstant(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        transform.rotation = gb.gb.transform.rotation;
    }

    public void SetSelectedObject()
    {
        if (CreatorManager.instance.isSelecting)
        {
            CreatorManager.instance.SetSelectedObject(gb);
        }
    }
}
