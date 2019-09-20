using System.Collections;
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
    public string cutSceneName;
    public bool startWithCutscene;
    public bool isEnabled;

    public GameObject[] diamond_Fills;

    public GameObject crateImage;
    public TextMeshProUGUI text;

    public void SetLevelSelection()
    {
        /*if (Client.instance != null)
            Client.client.WriteMessage("!disconnect!");*/
        Level_Selector_GB.instance.texture = texture;
        Level_Selector_GB.instance.textFile = file;
        Level_Selector_GB.instance.background = background;
        if (startWithCutscene)
        {
            Level_Selector_GB.instance.isCutscene = true;
            SceneManager.LoadScene(cutSceneName);
        }
        else
            SceneManager.LoadScene("SampleScene");
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
