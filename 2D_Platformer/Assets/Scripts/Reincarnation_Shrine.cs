using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reincarnation_Shrine : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public void ReincarnateBoss(GameObject prefab)
    {
        Instantiate(prefab, transform.position+Vector3.up*10f, Quaternion.identity);
        Instantiate(mushroomPrefab, transform.position + Vector3.up * 2f,Quaternion.identity);
    }
}
