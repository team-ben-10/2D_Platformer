using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject floorEndCheck;
    protected Rigidbody2D rb;
    public GameObject onWallCheck;
    public LayerMask evoidMask;
    public LayerMask notEnemyMask;
    public float speed;
    public bool canRunOverEdges;

    public virtual void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
    }

    bool isMovingLeft;

    protected virtual void Update()
    {
        {
            if (!canRunOverEdges)
            {
                var colls = Physics2D.OverlapCircleAll(floorEndCheck.transform.position, 0.01f,notEnemyMask);
                if (colls.Length <= 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    isMovingLeft = !isMovingLeft;
                }
            }
        }
        {
            var colls = Physics2D.OverlapCircleAll(onWallCheck.transform.position, 0.1f, evoidMask);
            if (colls.Length > 0)
            {
                OnWallHit();
            }
        }
        Move();
    }

    public virtual void OnWallHit()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        isMovingLeft = !isMovingLeft;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.StartsWith("Player"))
        {
            onTouch(collision.gameObject, collision);
        }
    }

    protected virtual void Move()
    {
        rb.velocity = new Vector2((isMovingLeft ? 1 : -1) * speed, rb.velocity.y);
    }

    protected virtual void onTouch(GameObject player, Collider2D col)
    {
        Debug.Log("Touch");
    }
}
