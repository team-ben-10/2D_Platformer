using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.StartsWith ("Player"))
        {
            collision.GetComponent<PlayerStats>().Die();
        }
    }
}
