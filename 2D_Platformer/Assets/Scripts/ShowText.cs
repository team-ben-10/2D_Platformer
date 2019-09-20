using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    GameObject textPanel;
    string text = "";
    public int NBTValue;
    private void Start()
    {
        if(NBTValue == 0)
            NBTValue = GetComponent<AlphaNBTTag>().NBT;
        textPanel = GameObject.FindGameObjectWithTag("Text_Panel").transform.GetChild(0).gameObject;
    }

    public void UpdateText()
    {
        var lines = GameManager.instance.textFile.text.Split('\n');
        foreach (var line in lines)
        {
            if (line.StartsWith(NBTValue + ":"))
            {
                text = line.Replace(NBTValue + ":", "");
            }
        }
        textPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
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
