using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Dungeon_1_Final : Lever
{
    public override void OnTriggerExit2D(Collider2D coll)
    {
        if (GameManager.instance.currentQuests.Exists((x) => x.name == "From The Skies (2)"))
        {
            Quest q = GameManager.instance.currentQuests.Find((x) => x.name == "From The Skies (2)");
            q.CompleteQuestStep();
            q.isFinished();
        }
        StartCoroutine(playCutScene());
        return;
    }

    bool isPlaying = false;

    IEnumerator playCutScene()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            GameObject player1 = GameObject.FindGameObjectWithTag("Player");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player_2");
            StartCoroutine(GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy_Dungeon_1_Boss>().Activated());
            if (player1 != null)
            {
                player1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player1.GetComponent<PlayerMovement>().enabled = false;
                player1.GetComponent<PlayerStats>().enabled = false;
            }
            if (player2 != null)
            {
                player2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player2.GetComponent<PlayerMovement>().enabled = false;
                player2.GetComponent<PlayerStats>().enabled = false;
            }
            yield return new WaitForSeconds(1f);
            if (player1 != null)
            {
                player1.GetComponent<PlayerMovement>().enabled = true;
                player1.GetComponent<PlayerStats>().enabled = true;
            }
            if (player2 != null)
            {
                player2.GetComponent<PlayerMovement>().enabled = true;
                player2.GetComponent<PlayerStats>().enabled = true;
            }
            Destroy(gameObject);
        }
    }
}
