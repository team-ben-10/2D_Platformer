using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy_MiniBoss : Enemy
{
    int lives = 0;
    public int maxLives;
    bool wasDamaged;
    public int invincibleTime;
    public string bossName;
    public float spawnTime;
    public string questNameFinished;

    protected override void Move()
    {
        base.Move();
    }

    public override void OnWallHit()
    {
        base.OnWallHit();
    }

    IEnumerator NotDamagable()
    {
        for (int i = 0; i < invincibleTime*8f; i++)
        {
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
            yield return new WaitForSeconds(1 / 8f);
        }
        gameObject.GetComponent<Renderer>().enabled = true;
        wasDamaged = false;
    }

    protected override void onTouch(GameObject player, Collider2D col)
    {
        if(lives > 0)
        {
            if (!wasDamaged)
            {
                if (player.GetComponent<PlayerStats>().mushroomEffect != null)
                {
                    if (player.GetComponent<PlayerStats>().mushroomEffect is VisInstaKillMushroomEffect)
                    {
                        player.GetComponent<Rigidbody2D>().velocity = Vector3.up * 15f;
                        lives--;
                        Debug.Log(bossName + " was Damaged");
                        wasDamaged = true;
                        StartCoroutine(NotDamagable());
                    }
                    else
                    {
                        OnPlayerDefeated();
                    }
                }
                else
                {
                    OnPlayerDefeated();
                }
            }
            else
            {
                OnPlayerDefeated();
            }
        }
        if(lives <= 0)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            GetComponent<Animator>().SetTrigger("Death");
            StopAllCoroutines();
            Invoke("OnDefeat", 1f);
        }
    }

    public virtual void OnPlayerDefeated()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().Die();
    }

    public virtual void OnDefeat()
    {
        GameManager.instance.currentQuests.Find((x) => x.name == questNameFinished).CompleteQuestStep();
        GameManager.instance.currentQuests.Find((x) => x.name == questNameFinished).isFinished();
        Destroy(gameObject);
    }

    public virtual IEnumerator OnAttack()
    {
        yield return null;
        StartCoroutine(OnAttack());
    }

    float oldGravityScale = 0;

    public virtual void OnSpawn()
    {
        oldGravityScale = GetComponent<Rigidbody2D>().gravityScale;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public virtual void OnSpawnEnd()
    {
        GetComponent<Rigidbody2D>().gravityScale = oldGravityScale;
    }

    public override void Start()
    {
        base.Start();
        OnSpawn();
        Invoke("OnSpawnEnd", spawnTime);
        lives = maxLives;
        StartCoroutine(OnAttack());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" || collision.transform.tag == "Player_2")
        {
            OnPlayerDefeated();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
