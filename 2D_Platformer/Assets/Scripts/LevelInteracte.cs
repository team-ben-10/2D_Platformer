using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInteracte : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] renderers;
    [SerializeField] Collider2D[] colliders;
    [SerializeField] Behaviour[] behaviours;
    [SerializeField] bool Active;
    [SerializeField] bool notstartActive;
    [HideInInspector]public int NBT;
    void Start()
    {
        NBT = GetComponent<AlphaNBTTag>().NBT;
        if (!notstartActive)
            InteractOff();
    }
    
    public virtual void InteractOn()
    {
        foreach (var item in renderers)
        {
            item.enabled = !Active;
        }
        foreach (var item in colliders)
        {
            item.enabled = !Active;
        }
        foreach (var item in behaviours)
        {
            item.enabled = !Active;
        }
    }

    public virtual void InteractOff()
    {
        foreach (var item in renderers)
        {
            item.enabled = Active;
        }
        foreach (var item in colliders)
        {
            item.enabled = Active;
        }
        foreach (var item in behaviours)
        {
            item.enabled = !Active;
        }
    }
}
