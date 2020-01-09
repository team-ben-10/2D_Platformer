using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwapper : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
            if (rb.gravityScale > 0) {
                rb.gravityScale *= -1;
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
            if (rb.gravityScale < 0)
            {
                rb.gravityScale *= -1;
            }
    }
}
