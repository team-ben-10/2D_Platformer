using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRight : MonoBehaviour
{
    public float timeBetweenSpawns;
    public GameObject prefabs;
    float currentTime;
    public float DestroyTime = 0;
    void Start()
    {
        currentTime = timeBetweenSpawns;
        
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            currentTime = timeBetweenSpawns;
            GameObject gb = Instantiate(prefabs, transform.position , Quaternion.identity);
            if (DestroyTime > 0)
            {
                Destroy(gb, DestroyTime);
            }
        }
    }


}
