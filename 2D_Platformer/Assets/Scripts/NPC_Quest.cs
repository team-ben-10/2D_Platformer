using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowText))]
public class NPC_Quest : MonoBehaviour
{
    public string questName;
    public string lastQuestName;
    public bool onlyShowWithQuest;
    public string ShowWithQuestName;
    public int acceptedNBT;
    public int notFinishedNBT;
    public int FinishedNBT;

    private void Update()
    {
        if(onlyShowWithQuest)
            if(ShowWithQuestName != "")
            {
                if(!GameManager.instance.currentQuests.Exists((x) => x.name == ShowWithQuestName))
                {
                    Destroy(gameObject);
                }
            }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (GameManager.instance.currentQuests.Exists((x) => x.name == lastQuestName))
            {
                GameManager.instance.currentQuests.Find((x) => x.name == lastQuestName).CompleteQuestStep();
                GetComponent<ShowText>().NBTValue = FinishedNBT;
                GetComponent<ShowText>().UpdateText();
                return;
            }
            if (!GameManager.instance.currentQuests.Exists((x)=>x.name == questName))
            {
                GameManager.instance.AddQuest(questName);
                GetComponent<ShowText>().NBTValue = acceptedNBT;
                GetComponent<ShowText>().UpdateText();
                return;
            }
            else
            { 
                GetComponent<ShowText>().NBTValue = notFinishedNBT;
                GetComponent<ShowText>().UpdateText();
                return;
            }
        }
    }
}
