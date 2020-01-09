using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string questName;
    bool used = false;
    public bool questItemPickup;

    void Start()
    {
        if(!GameManager.instance.currentQuests.Exists((x)=>x.name == questName) && questItemPickup)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.StartsWith("Player"))
        {
            PickUP();
        }
    }

    void AddToQuest()
    {
        GameManager.instance.CheckQuests(questName);
    }

    void PickUP()
    {
        if (!used)
        {
            GetComponent<Animator>().SetBool("Pickup", true);

            Quest q = GameManager.instance.currentQuests.Find(x => x.name == questName);
            
            if (q == null)
            {
                GameManager.instance.AddQuest(questName);
                GameManager.instance.CheckQuests(questName);
                Destroy(gameObject, 1f);
            }
            else
            {
                CollectableQuest collectableQuest = (CollectableQuest)q;
                if (questItemPickup)
                {
                    collectableQuest.collectableAmount++;
                    GameManager.instance.CheckQuests(questName);
                    Destroy(gameObject, 1f);
                }
            }

            used = true;
        }
        /*if (!used)
        {
            GetComponent<Animator>().SetBool("Pickup", true);
            used = true;
            foreach (var item in GameManager.instance.allQuests)
            {
                if (item.name == questName)
                {
                    if (!GameManager.instance.currentQuests.Contains(item))
                    {
                        if (!GameManager.instance.IsUnloced(item.unlocableName[0]))
                        {
                            string path = Application.persistentDataPath + "/Quests/" + item.name + ".quest";
                            GameManager.instance.currentQuests.Add(item);
                            File.Create(path).Close();
                            File.WriteAllLines(path, new string[] { "unlocables:" + item.AmountForUnlocable, "completed:" + item.isCompleted });
                            Destroy(gameObject, 1f);
                        }
                    }
                    else
                    {
                        if (questItemPickup)
                        {
                            GameManager.instance.onLevelCleared += AddToQuest;
                            Destroy(gameObject, 1f);
                        }
                    }
                }
            }
        }*/
    }
}
