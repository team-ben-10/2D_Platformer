using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public Vector2 vel;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Conveyor"))
        {
            collision.transform.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }
}
