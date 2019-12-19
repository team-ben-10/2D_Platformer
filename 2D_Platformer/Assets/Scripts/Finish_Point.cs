using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish_Point : MonoBehaviour
{
    [HideInInspector]
    public bool isTriggered = false;
    public List<StringQuest> questOnComplete;
    [System.Serializable]
    public class StringQuest
    {
        public int alpha;
        public string quest;
        public int timeToComplete;
        public bool completed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("Player") && !isTriggered)
        {
            if (GetComponent<AlphaNBTTag>() != null)
            {
                int nbt = GetComponent<AlphaNBTTag>().NBT;
                StringQuest q = questOnComplete.Find((x) => x.alpha == nbt);
                if (q != null)
                    if (GameManager.instance.time >= q.timeToComplete)
                    {
                        if (!q.completed)
                            GameManager.instance.AddQuest(q.quest);
                        else
                        {
                            GameManager.instance.currentQuests.Find((x) => x.name == q.quest).CompleteQuestStep();
                            GameManager.instance.currentQuests.Find((x) => x.name == q.quest).isFinished();
                        }
                    }
            }
            CoinManager.instance.SaveCoins();
            isTriggered = true;
            GameManager.instance.Win();
        }
    }

}
