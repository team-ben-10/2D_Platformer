using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindingPrefab : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI keyNameTitle;
    [SerializeField] TMPro.TextMeshProUGUI keyNameButton;

    [HideInInspector]public KeyCode key;
     public string keyName;

    bool checkKey = false;
    
    public void CheckKey()
    {
        checkKey = true;
        keyNameButton.text = "Press any Key...";
    }

    private void Update()
    {
        if (checkKey)
        {
            KeyCode key = InputManager.instance.GetPressedKey();
            if(key != KeyCode.None)
            {
                this.key = key;
                if (keyName.Contains("Pos"))
                {
                    InputManager.instance.GetPreset("Player").FindAxis(keyName.Replace("_Pos","")).posKey = key;
                }
                else if (keyName.Contains("Neg"))
                {
                    InputManager.instance.GetPreset("Player").FindAxis(keyName.Replace("_Neg", "")).negKey = key;
                }
                else
                {
                    InputManager.instance.GetPreset("Player").Find(keyName).key = key;
                }
                keyNameButton.text = key.ToString();
                checkKey = false;
                Debug.Log(key);
            }
        }
    }

    public void Setup()
    {
        keyNameTitle.text = keyName.Replace("_", " ").Replace("Horizontal","Movement").Replace("Pos", "Right").Replace("Neg","Left");
        keyNameButton.text = key.ToString();
    }
}
