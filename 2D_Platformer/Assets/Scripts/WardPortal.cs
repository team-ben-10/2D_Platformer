using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WardPortal : MonoBehaviour
{
    public KeyCode key;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(key))
            {
                LoadManager.Instance.LoadScene("Wardrobe");
            }
        }
    }
}
