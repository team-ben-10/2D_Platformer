using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MushroomEffect : MonoBehaviour
{

    public virtual void OnStart(GameObject player)
    {
    }

    public virtual void OnEnd(GameObject player, float timeLeft, int oldNBT)
    {
    }

    public virtual void OnDraw(GameObject player, float TimeLeft)
    {
    }

    public float Time;

    private bool isUsed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag.StartsWith("Player")) && !isUsed)
        {
            for (int i = 2; i <= collision.GetComponent<PlayerMovement>().MaxPlayerAtATime; i++)
            {
                var gb = GameObject.FindGameObjectWithTag("Player_" + i);
                if (gb != null)
                {
                    gb.GetComponent<PlayerStats>().PickupItem(this);
                }
            }
            collision.GetComponent<PlayerStats>().PickupItem(this);


            gameObject.SetActive(false);
            isUsed = true;
        }
    }
}
