using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Selector : MonoBehaviour
{
    [System.Serializable]
    public class World
    {
        public string worldName;
        public int starAmount;
        public List<Level> levels = new List<Level>();
        
    }

    [System.Serializable]
    public class Level
    {
        public string levelName;
        public Texture2D texture;
        public TextAsset textFile;
        public Sprite background_Image;
        public Sprite previewImage;
        public bool startWithCutscene;
        [Header("Audio")]
        public string clipName;
        [Header("Cutescene Settings")]
        public string cutSceneName;
    }

    public List<World> worlds = new List<World>();
    public GameObject Level_Select_Prefab;
    public GameObject World_Prefab;
    public Transform Content;
    public int lastStarAmount = 3;
    [HideInInspector] public int lastStarSumAmount = 0;



    void Start()
    {

        int worldIndex = 1;
        int levelIndex = 1;
        foreach (var world in worlds)
        {
            GameObject wGB = Instantiate(World_Prefab, Content);
            GameObject worldGB = wGB.transform.GetChild(0).gameObject;
            if (lastStarSumAmount >= world.starAmount)
            {
                foreach (var item in world.levels)
                {
                    
                    GameObject gb = Instantiate(Level_Select_Prefab, worldGB.transform);
                    gb.GetComponent<Image>().sprite = item.previewImage;
                    var lsp = gb.GetComponent<Level_Selection_Prefab>();
                    lsp.texture = item.texture;
                    lsp.file = item.textFile;
                    lsp.background = item.background_Image;
                    lsp.cutSceneName = item.cutSceneName;
                    lsp.startWithCutscene = item.startWithCutscene;
                    lsp.clipName = item.clipName;

                    if (item.textFile != null)
                    {
                        string path = Application.persistentDataPath + "/" + item.textFile.name + ".txt";
                        int star = 0;
                        if (File.Exists(path))
                        {
                            string[] lines = File.ReadAllLines(path);
                            foreach (var line in lines)
                            {
                                if (line.StartsWith("starAmount:"))
                                {
                                    star = int.Parse(line.Replace("starAmount: ", ""));
                                }
                            }
                        }

                        lsp.setDiamonds(star);
                        if (lastStarAmount == 0)
                        {
                            lsp.notUsable();
                        }
                        lsp.setName(worldIndex + " - " + levelIndex);
                        lastStarAmount = star;
                        lastStarSumAmount += star;
                    }
                    else
                    {
                        lsp.notUsable();
                    }
                    levelIndex++;
                }
            }
            else
            {
                foreach (var item in world.levels)
                {
                    GameObject gb = Instantiate(Level_Select_Prefab, worldGB.transform);
                    gb.GetComponent<Level_Selection_Prefab>().notUsable();
                }
            }
            //worldGB.GetComponent<ScrollRect>().GraphicUpdateComplete();
            worldIndex++;
            levelIndex = 1;
        }
        transform.GetComponent<ScrollRect>().Rebuild(CanvasUpdate.MaxUpdateValue);
    }

    public void ResetScores()
    {
        GameManager.instance.ResetUnlocables();
        foreach (var world in worlds)
        {
            foreach (var item in world.levels)
            {
                string path = Application.persistentDataPath + "/" + item.textFile.name + ".txt";
                if (File.Exists(path))
                    File.Delete(path);
                LoadManager.Instance.LoadScene("Start_Scene");
            }
        }
    }



    public void UnlocAll()
    {
        foreach (var world in worlds)
        {
            foreach (var item in world.levels)
            {
                string path = Application.persistentDataPath + "/" + item.textFile.name + ".txt";
                if (!File.Exists(path))
                    File.Create(path).Close();
                File.WriteAllLines(path, new string[] { "bestTime: 0", "starAmount: 3" });
            }
        }
    }
}
