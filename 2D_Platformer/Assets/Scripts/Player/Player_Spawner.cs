using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawner : MonoBehaviour
{
    public GameObject Player;
    public void SpawnPlayer()
    {
        Instantiate(Player, transform.position,Quaternion.identity);
    }
}
