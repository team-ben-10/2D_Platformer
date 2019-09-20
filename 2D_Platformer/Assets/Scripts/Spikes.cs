using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().Die();
        }
        if (collision.tag == "Player_2")
        {
            collision.GetComponent<PlayerStats>().Die();
        }
    }
}
