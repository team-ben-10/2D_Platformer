using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Play_BTN : MonoBehaviour
{
    bool started = false;
    public TextMeshProUGUI text;
    public void Click()
    {
        started = !started;
        text.text = started ? "End" : "Play";
        CreatorManager.instance.SetTo(started);
    }

    private void Update()
    {
        if(started != CreatorManager.instance.isPlayMode)
        {
            started = CreatorManager.instance.isPlayMode;
            text.text = started ? "End" : "Play";
        }
    }
}
