using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public GameObject gb;
    
    void Start()
    {
        if(GameManager.instance.currentQuests.Count <= 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("QuestInfo"));
            Destroy(gameObject);
            return;
        }
        foreach (var item in GameManager.instance.currentQuests)
        {
            GameObject g = Instantiate(gb, transform);
            g.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.name;
            g.GetComponent<QuestShow>().Initialize(item);
        }
        GameObject.FindGameObjectWithTag("QuestInfo").SetActive(false);
    }
}
