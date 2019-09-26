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
    Image completedImage;
    GameObject gb;
    Quest quest;

    public void Initialize(Quest q)
    {
        quest = q;
        
        gb = GameObject.FindGameObjectWithTag("QuestInfo");
        titletext = gb.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        infotext = gb.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        collectedtext = gb.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        completedImage = gb.transform.GetChild(3).GetComponent<Image>();
    }

    public void Show()
    {
        gb.SetActive(true);
        titletext.text = quest.name;
        infotext.text = quest.infoText;
        /*if (quest.maxAmountForUnlocable > 0)
        {
            collectedtext.text = quest.AmountForUnlocable + "/" + quest.maxAmountForUnlocable;
        }
        else
        {
            */collectedtext.gameObject.SetActive(false);/*
        }*/
        /*if (!quest.needsSomethingElseCompleted)
        {
            completedImage.gameObject.SetActive(false);
        }
        else
        {*/
            completedImage.gameObject.SetActive(true);
            if (quest.isCompleted)
                completedImage.sprite = correct;
        //}
    }
}
