using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shrine : MonoBehaviour
{
    [System.Serializable]
    private struct QuestValue
    {
        public string name;
        public int value;
        public UnityEvent events;
    }

    private bool used = false;
    [SerializeField] private List<QuestValue> questValues;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!used)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                foreach (var item in questValues)
                {
                    if (GameManager.instance.currentQuests.Exists((x) => x.name == item.name))
                    {
                        if(item.value != 0)
                            GameManager.instance.CheckQuests(item.name/*,item.value*/);
                        item.events.Invoke();
                        Debug.Log("Added Value to " + item.name);
                    }
                }
                used = true;
            }
        }
    }
}
