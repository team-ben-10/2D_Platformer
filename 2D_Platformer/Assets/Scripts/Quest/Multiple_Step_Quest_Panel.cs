using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiple_Step_Quest_Panel : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Content;
    public Sprite QuestStepCompletedIcon;

    public void Setup(List<MultipleStepQuest.QuestStep> steps)
    {
        foreach (var item in steps)
        {
            var gb = Instantiate(Prefab,Content);
            gb.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.text;
            if (item.completed)
                gb.transform.GetChild(1).GetComponent<Image>().sprite = QuestStepCompletedIcon;
        }
    }
}
