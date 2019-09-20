using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwapper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Player_2")
        {
            collision.GetComponent<Rigidbody2D>().velocity += Vector2.up * 150f;
        }
    }
}
