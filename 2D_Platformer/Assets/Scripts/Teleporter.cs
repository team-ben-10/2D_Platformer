using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    int nbt;

    private void Start()
    {
        nbt = GetComponent<AlphaNBTTag>().NBT;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            foreach (var item in FindObjectsOfType<Teleporter>())
            {
                if (item != this)
                {
                    if (item.GetComponent<AlphaNBTTag>().NBT == nbt)
                    {
                        if (GameObject.FindGameObjectWithTag("Player") != null)
                        {
                            GameObject.FindGameObjectWithTag("Player").transform.position = item.transform.position + Vector3.up;
                        }
                        if (GameObject.FindGameObjectWithTag("Player_2") != null)
                        {
                            GameObject.FindGameObjectWithTag("Player_2").transform.position = item.transform.position + Vector3.up;
                        }
                    }
                }
            }
        }
    }
}
