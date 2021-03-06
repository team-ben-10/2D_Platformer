﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Selection_Prefab : MonoBehaviour
{
    public TextAsset file;
    public Texture2D texture;
    public Sprite background;
    public string clipName;
    public string cutSceneName;
    public bool startWithCutscene;
    public bool isEnabled;

    public GameObject[] diamond_Fills;

    public GameObject crateImage;
    public TextMeshProUGUI text;
    public string levelName;

    public void SetLevelSelection()
    {
        /*if (Client.instance != null)
            Client.client.WriteMessage("!disconnect!");*/
        Level_Selector_GB.instance.texture = texture;
        Level_Selector_GB.instance.textFile = file;
        Level_Selector_GB.instance.background = background;
        Level_Selector_GB.instance.clipName = clipName;
        Level_Selector_GB.instance.levelName = levelName;
        if (startWithCutscene)
        {
            Level_Selector_GB.instance.isCutscene = true;
            LoadManager.Instance.background.sprite = background;
            LoadManager.Instance.levelText.text = levelName;
            LoadManager.Instance.LoadScene(cutSceneName);
        }
        else
        {
            LoadManager.Instance.background.sprite = background;
            LoadManager.Instance.levelText.text = levelName;
            LoadManager.Instance.LoadScene("SampleScene");
        }
    }

    public void setDiamonds(int diamonds)
    {
        for (int i = 0; i < diamonds; i++)
        {
            diamond_Fills[i].SetActive(true);
        }
    }

    public void notUsable()
    {
        if (!isEnabled)
        {
            crateImage.SetActive(true);
            GetComponent<Button>().enabled = false;
        }
    }

    public void setName(string s)
    {
        text.text = s;
    }
}
