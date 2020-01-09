using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisBlock : MonoBehaviour
{
    bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            gameObject.SetActive(false);
        }
    }

}
