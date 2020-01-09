using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShowText : MonoBehaviour
{
    GameObject textPanel;
    string text = "";
    public int NBTValue;
    private void Start()
    {
        if (NBTValue == 0)
            NBTValue = GetComponent<AlphaNBTTag>().NBT;
        textPanel = GameObject.FindGameObjectWithTag("Text_Panel").transform.GetChild(0).gameObject;
        textField = textPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    IEnumerator DisplayText()
    {
        var lines = GameManager.instance.textFile.text.Split('\n');
        foreach (var line in lines)
        {
            if (line.StartsWith(NBTValue + ":"))
            {
                text = line.Replace(NBTValue + ":", "");
            }
        }

        var final = "";
        var playerPreset = InputManager.instance.GetPreset("Player");
        var splits = text.Split(' ');
        foreach (var item in splits)
        {
            if(item.StartsWith("[") && item.EndsWith("]"))
            {
                var button = item.Substring(1, item.Length - 2);
                Debug.Log(button);
                var key = playerPreset.Find(button)?.key;
                if (key != null)
                    final += "[" + key.ToString() + "] ";
                var axis = playerPreset.FindAxis(button);
                if (axis != null)
                    final += "[" + axis.negKey.ToString() + "] [" + axis.posKey.ToString() + "] ";
            }
            else
            {
                final += item + " ";
            }
        }

        final = final.Trim();



        foreach (var c in final)
        {
            textField.text += c;
            yield return null;
        }
    }

    TextMeshProUGUI textField;
    Coroutine old;

    public void UpdateText()
    {
        if (old != null)
            StopCoroutine(old);
        textField.text = "";
        old = StartCoroutine(DisplayText());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UpdateText();
            textPanel.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            textPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            textPanel.SetActive(false);
        }
    }
}
