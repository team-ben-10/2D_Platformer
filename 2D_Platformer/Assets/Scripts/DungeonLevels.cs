using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DungeonLevels : MonoBehaviour
{
    [System.Serializable]
    public class DungeonLevel
    {
        public string name;
        public Texture2D texture;
        public TextAsset textFile;
    }

    public Sprite background;

    public List<DungeonLevel> levels = new List<DungeonLevel>();
    
    public void SelectLevel()
    {
        
        var selector = GetComponent<Level_Selection_Prefab>();
        DungeonLevel level = SelectLeveFromWeek();
        if (level == null)
            return;
        string path = Application.persistentDataPath + "/Dungeon.dat";
        if (!File.Exists(path))
        {
            File.Create(path).Close();
            File.WriteAllText(path, "LastPlayed:" + System.DateTime.Now);
        }
        else
        {
            if (System.DateTime.Parse(File.ReadAllText(path).Replace("LastPlayed:", "")).AddDays(+1) > System.DateTime.Now)
            {
                //return;
            }
            else
            {
                File.WriteAllText(path, "LastPlayed:" + System.DateTime.Now);
            }
        }
        selector.file = level.textFile;
        selector.texture = level.texture;
        selector.background = background;
        selector.SetLevelSelection();
    }

    private DungeonLevel SelectLeveFromWeek()
    {
        var day = System.DateTime.Now.DayOfWeek;
        for (int i = 6; i > 0; i--)
        {
            if(((int)day + i) % 7 == 0)
                return levels[6-i];
        }
        return null;
    }
}
