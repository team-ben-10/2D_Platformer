using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pokey : Enemy
{
    bool alive = true;
    public GameObject BodyPartPrefab;
    public GameObject HeadPartPrefab;

    protected override void onTouch(GameObject player, Collider2D col)
    {
        if(alive)
        {
            player.GetComponent<PlayerStats>().Die();
        }
    }

    public override void Start()
    {
        base.Start();
        var nbt = GetComponent<AlphaNBTTag>();
        if (nbt == null)
        {
            nbt = gameObject.AddComponent<AlphaNBTTag>();
            nbt.setNBT(50);
        }
        for (int i = 0; i < (nbt.NBT/10)-1; i++)
        {
            var bodyPart = Instantiate(BodyPartPrefab, transform.position + new Vector3(0, i), Quaternion.identity, transform);
        }
        var headPart = Instantiate(HeadPartPrefab, transform.position + new Vector3(0, nbt.NBT / 10-1), Quaternion.identity, transform);
    }
}
