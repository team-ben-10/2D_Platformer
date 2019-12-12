using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Block : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rb = collision.transform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.velocity.y < 0)
            {
                var vel = rb.velocity;
                vel = new Vector2(vel.x, -vel.y);
                rb.velocity = vel;
            }
        }
    }
}
