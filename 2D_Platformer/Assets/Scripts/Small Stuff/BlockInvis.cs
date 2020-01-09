using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInvis : MonoBehaviour
{
    public Sprite Sprite;

    bool isTriggered = false;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            spriteRenderer.sprite = Sprite;
            
        }
    }
}
