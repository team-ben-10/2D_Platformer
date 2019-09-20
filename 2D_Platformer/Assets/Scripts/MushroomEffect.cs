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
        if ((collision.tag == "Player" || collision.tag == "Player_2") && !isUsed)
        {
            if(GameObject.FindGameObjectWithTag("Player_2") != null)
            {
                GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerStats>().PickupItem(this);
                collision.GetComponent<PlayerStats>().PickupItem(this);
            }
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().PickupItem(this);
                collision.GetComponent<PlayerStats>().PickupItem(this);
            }
            gameObject.SetActive(false);
            isUsed = true;
        }
    }
}
