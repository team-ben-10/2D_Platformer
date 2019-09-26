using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestShow : MonoBehaviour
{
    TMPro.TextMeshProUGUI infotext;
    TMPro.TextMeshProUGUI titletext;
    TMPro.TextMeshProUGUI collectedtext;
    public Sprite correct;
    GameObject multipleStepPanel;
    GameObject gb;
    Quest quest;

    public void Initialize(Quest q)
    {
        quest = q;
        gb = GameObject.FindGameObjectWithTag("QuestInfo");
        titletext = gb.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        infotext = gb.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        collectedtext = gb.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        //completedImage = gb.transform.GetChild(3).GetComponent<Image>();
        multipleStepPanel = gb.transform.GetChild(3).gameObject;
    }

    public void Show()
    {
        gb.SetActive(true);
        titletext.text = quest.name;
        infotext.text = quest.infoText;
        infotext.gameObject.SetActive(true);
        multipleStepPanel.SetActive(false);
        if (quest is CollectableQuest)
        {
            CollectableQuest collectableQuest = (CollectableQuest)quest;
            collectedtext.text = collectableQuest.collectableAmount + "/" + collectableQuest.MaxAmountCollectable;
            collectedtext.gameObject.SetActive(true);
        }
        else
        {
            collectedtext.gameObject.SetActive(false);
        }
        if(quest is MultipleStepQuest)
        {
            var script = multipleStepPanel.GetComponent<Multiple_Step_Quest_Panel>();
            for (int i = 0; i < script.Content.childCount; i++)
            {
                Destroy(script.Content.GetChild(i).gameObject);
            }

            MultipleStepQuest multipleStepQuest = (MultipleStepQuest)quest;
            multipleStepPanel.SetActive(true);
            script.Setup(multipleStepQuest.steps);
            infotext.gameObject.SetActive(false);
        }
    }
}
