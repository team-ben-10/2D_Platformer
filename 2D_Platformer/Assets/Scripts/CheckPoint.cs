using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isTriggered = false;
    public Sprite checkedSprite;
    public Sprite uncheckedSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player" || collision.tag == "Player_2") && !isTriggered)
        {
            isTriggered = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().lastCheckPoint = transform.position;
            if (checkedSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = checkedSprite;
            }
        }
    }
}
