using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    SpriteRenderer renderer;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (renderer != null)
        {
            bool isVisible = Camera.main.IsObjectVisible(renderer);
            renderer.enabled = isVisible;
        }
    }
}
