using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Multiple Step Quest", menuName = "Quests/New Multiple Step Quest")]
public class MultipleStepQuest : Quest
{
    [System.Serializable]
    public class QuestStep
    {
        public string text;
        public bool completed;

        public QuestStep(string text, bool completed)
        {
            this.text = text;
            this.completed = completed;
        }

        public QuestStep Copy()
        {
            return new QuestStep(text, completed);
        }
    }

    public List<QuestStep> steps = new List<QuestStep>();

    public void CompleteQuestStep(int i, bool value = true)
    {
        steps[i].completed = value;
    }

    public override void Setup(string[] lines)
    {
        base.Setup(lines);
        for (int i = 1; i < lines.Length; i++)
        {
            steps[i - 1].completed = bool.Parse(lines[i].Split(':')[1]);
        }
    }

    public override bool isFinished()
    {
        isCompleted = true;
        foreach (var item in steps)
        {
            if (!item.completed)
                isCompleted = false;
        }
        return base.isFinished();
    }

    public override string[] GetSaveString()
    {
        string[] lines = new string[steps.Count + 1];
        lines[0] = "completed:" + isCompleted;
        for (int i = 1; i < lines.Length; i++)
        {
            lines[i] = (i - 1) + ":" + steps[i - 1].completed;
        }
        return lines;
    }

    public override Quest Copy()
    {
        MultipleStepQuest q = (MultipleStepQuest)base.Copy();
        List<QuestStep> steps = new List<QuestStep>();
        foreach (var item in this.steps)
        {
            steps.Add(item.Copy());
        }
        q.steps = steps;
        return q;
    }
}
