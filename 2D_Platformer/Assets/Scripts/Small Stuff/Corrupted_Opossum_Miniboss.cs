using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corrupted_Opossum_Miniboss : Enemy_MiniBoss
{
    bool canMove = true;
    public override IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3.5f));
        canMove = false;
        yield return new WaitForSeconds(.25f);
        GetComponent<Rigidbody2D>().velocity += Vector2.up * 20f;
        canMove = true;
        StartCoroutine(OnAttack());
    }

    protected override void Move()
    {
        if(canMove)
            base.Move();
    }

    public override void OnPlayerDefeated()
    {
        base.OnPlayerDefeated();
        foreach (var item in GameObject.FindObjectsOfType<VisInstaKillMushroomEffect>())
        {
            Destroy(item.gameObject);
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ResetMushroomEffect();
        if(GameObject.FindGameObjectWithTag("Player_2") != null)
            GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerStats>().ResetMushroomEffect();
        Destroy(gameObject);
    }
}
