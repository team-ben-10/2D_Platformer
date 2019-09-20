using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public List<string> unlocableName;
    [Multiline]
    public string infoText;
    public bool finished;
    public int maxAmountForUnlocable;
    [HideInInspector]public int AmountForUnlocable;
    public bool needsSomethingElseCompleted;
    [HideInInspector]
    public bool isCompleted;
    public string questAfter;

    public void Setup()
    {
        AmountForUnlocable = 0;
    }

    public void CompleteQuestStep()
    {
        isCompleted = true;
    }

    public bool isFinished()
    {
        if (((maxAmountForUnlocable > 0)?AmountForUnlocable >= maxAmountForUnlocable : true) && (needsSomethingElseCompleted ? isCompleted : true))
        {
            finished = true;
            foreach (var item in unlocableName)
            {
                GameManager.instance.SetUnlocable(item, true);
            }
            if (questAfter != "")
            {
                GameManager.instance.AddQuest(questAfter);
            }
            string path = Application.persistentDataPath + "/Quests/" + name + ".quest";
            File.Delete(path);
            GameManager.instance.currentQuests.Remove(this);
            Debug.Log("Finished Quest " + name);
            return true;
        }
        return false;
    }

    public bool CheckForFinishedAdd(int amount = 1)
    {
        AmountForUnlocable+=amount;
        return isFinished();
    }
}
