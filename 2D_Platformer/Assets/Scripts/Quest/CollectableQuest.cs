using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectable Quest", menuName = "Quests/New Collectable Quest")]
public class CollectableQuest : Quest
{
    [HideInInspector]public int collectableAmount;
    public int MaxAmountCollectable;

    public override void Setup(string[] lines)
    {
        foreach (var item in lines)
        {
            if (item.StartsWith("completed:"))
            {
                isCompleted = bool.Parse(item.Replace("completed:", ""));
            }
            if (item.StartsWith("collectables:"))
            {
                collectableAmount = int.Parse(item.Replace("collectables:", ""));
            }
        }
    }

    public override bool isFinished()
    {
        if (collectableAmount >= MaxAmountCollectable || isCompleted)
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

    public override string[] GetSaveString()
    {
        return new string[] { "collectables:" + collectableAmount,"completed:" + isCompleted };
    }

    public override Quest Copy()
    {
        CollectableQuest q = (CollectableQuest)base.Copy();
        q.MaxAmountCollectable = MaxAmountCollectable;
        return q;
    }
}
