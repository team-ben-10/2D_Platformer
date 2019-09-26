using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/New Quest")]
public class Quest : ScriptableObject
{
    public List<string> unlocableName;
    [Multiline]
    public string infoText;
    //public bool finished;
    //[HideInInspector]public int AmountForUnlocable;
    //public bool needsSomethingElseCompleted;
    [HideInInspector]
    public bool isCompleted;
    public Quest questAfter;

    public virtual void Setup(string[] lines)
    {
        foreach (var item in lines)
        {
            if (item.StartsWith("completed:"))
            {
                isCompleted = bool.Parse(item.Replace("completed:", ""));
            }
        }
        //AmountForUnlocable = 0;
    }

    public void CompleteQuestStep()
    {
        isCompleted = true;
    }

    protected virtual void DeleteOldData()
    {
        string path = Application.persistentDataPath + "/Quests/" + name + ".quest";
        File.Delete(path);
        GameManager.instance.currentQuests.Remove(this);
        Debug.Log("Finished Quest " + name);
    }

    public virtual bool isFinished()
    {
        if (/*((maxAmountForUnlocable > 0)?AmountForUnlocable >= maxAmountForUnlocable : true) &&*/isCompleted)
        {
            foreach (var item in unlocableName)
            {
                GameManager.instance.SetUnlocable(item, true);
            }
            if (questAfter != null)
            {
                GameManager.instance.AddQuest(questAfter);
            }
            DeleteOldData();
            return true;
        }
        return false;
    }

    public virtual string[] GetSaveString()
    {
        return new string[] { "completed:" + isCompleted };
    }

    public virtual bool Check()
    {
        return isFinished();
    }
}
