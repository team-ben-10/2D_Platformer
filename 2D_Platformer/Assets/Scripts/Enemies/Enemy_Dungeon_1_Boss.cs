using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dungeon_1_Boss : Enemy
{

    bool canMove = true;
    bool wasDamaged = false;

    float oldSpeed = 0;
    float oldTime = 0;
    float oldDeltaTime = 0;
    protected override void onTouch(GameObject player, Collider2D col)
    {
        if (lives <= 0)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (allTraps.Contains(item.GetComponent<LevelInteracte>().NBT) || allSignals.Contains(item.GetComponent<LevelInteracte>().NBT) || item.GetComponent<LevelInteracte>().NBT == mushrooms || item.GetComponent<LevelInteracte>().NBT == 100 || item.GetComponent<LevelInteracte>().NBT == 25)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
            }
            Invoke("Finish", 1f);
            oldTime = Time.timeScale;
            oldDeltaTime = Time.fixedDeltaTime;
            Time.timeScale *= 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.2f;
            GetComponent<Animator>().SetTrigger("Death");
        }
        else
        {
            if (!wasDamaged)
            {
                lives--;
                foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
                {
                    if (allTraps.Contains(item.GetComponent<LevelInteracte>().NBT) || item.GetComponent<LevelInteracte>().NBT == mushrooms || allSignals.Contains(item.GetComponent<LevelInteracte>().NBT))
                    {
                        item.GetComponent<LevelInteracte>().InteractOff();
                    }
                }
                player.GetComponent<PlayerStats>().ResetMushroomEffect();
                player.GetComponent<Rigidbody2D>().velocity += new Vector2((Random.Range(-1,1.1f))*35f, 35f);
                wasDamaged = true;
                speed *= 1.35f;
                StopCoroutine(damagePhase);
                damagePhase = StartCoroutine(AbilityGravity());
            }
        }
    }

    public void Finish()
    {
        Destroy(gameObject);
        Time.fixedDeltaTime = oldDeltaTime;
        Time.timeScale = oldTime;
        //Unloce new Skin or something
        GameManager.instance.Win();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ResetOnDeath(collision.gameObject);
        
    }

    private void ResetOnDeath(GameObject gb)
    {
        speed = oldSpeed;
        foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
        {
            if (allTraps.Contains(item.GetComponent<LevelInteracte>().NBT) || allSignals.Contains(item.GetComponent<LevelInteracte>().NBT) || item.GetComponent<LevelInteracte>().NBT == mushrooms || item.GetComponent<LevelInteracte>().NBT == 100 || item.GetComponent<LevelInteracte>().NBT == 25)
            {
                item.GetComponent<LevelInteracte>().InteractOff();
            }
        }
        if (gb.transform.tag == "Player" || gb.transform.tag == "Player_2")
            gb.transform.GetComponent<PlayerStats>().Die();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.GetComponent<PlayerStats>().Die();
        canMove = false;
        GetComponent<SpriteRenderer>().enabled = false;
        StopAllCoroutines();
        isActivated = false;
        lives = 3;
    }

    public override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().enabled = false;
        canMove = false;
        oldSpeed = speed;
    }

    bool isActivated = false;

    protected override void Update()
    {
        base.Update();
        if(player != null)
        {
            if (player.GetComponent<PlayerStats>().isDead)
            {
                ResetOnDeath(player);
            }
        }
    }

    GameObject player = null;

    public IEnumerator Activated()
    {
        if (!isActivated)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            isActivated = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().SetTrigger("Activate");
            yield return new WaitForSeconds(1);
            canMove = true;
            StartCoroutine(waitForSpecial());
            damagePhase = StartCoroutine(AbilityGravity());
        }
    }

    IEnumerator waitForSpecial()
    {
        yield return new WaitForSeconds(Random.Range(1.5f,5f));
        wasDamaged = false;
        canMove = false;
        for (int i = 0; i < 4-lives; i++)
        {
            int index = Random.Range(0, allTraps.Count - 1);
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (allSignals[index] == item.GetComponent<LevelInteracte>().NBT)
                {
                    item.GetComponent<LevelInteracte>().InteractOn();
                }
            }
            yield return new WaitForSeconds(1f);
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (allSignals[index] == item.GetComponent<LevelInteracte>().NBT)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
            }
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (allTraps[index] == item.GetComponent<LevelInteracte>().NBT)
                {
                    item.GetComponent<LevelInteracte>().InteractOn();
                }
            }
            yield return new WaitForSeconds(1f);
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (allTraps[index] == item.GetComponent<LevelInteracte>().NBT)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
            }
        }
        canMove = true;
        StartCoroutine(waitForSpecial());
    }

    protected override void Move()
    {
        if (canMove)
            base.Move();
        else
            rb.velocity = Vector2.zero;
    }

    int lives = 3;
    int mushrooms = 95;
    List<int> allTraps = new List<int>() { 90, 85, 80, 75, 70, 65, 60, 55};
    List<int> allSignals = new List<int>() { 91, 86, 81, 76, 71, 66, 61, 56};

    Coroutine damagePhase;

    IEnumerator AbilityGravity()
    {
        yield return new WaitForSeconds(30);
        canMove = false;
        foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
        {
            if (allSignals.Contains(item.GetComponent<LevelInteracte>().NBT) || item.GetComponent<LevelInteracte>().NBT == mushrooms || (lives==1? item.GetComponent<LevelInteracte>().NBT==25:false))
            {
                item.GetComponent<LevelInteracte>().InteractOn();
            }
        }
        yield return new WaitForSeconds((lives==1)?7.5f:5);
        canMove = true;
        foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
        {
            if (allSignals.Contains(item.GetComponent<LevelInteracte>().NBT) || (lives==1?item.GetComponent<LevelInteracte>().NBT==25:false))
            {
                item.GetComponent<LevelInteracte>().InteractOff();
            }
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
        {
            if (allTraps.Contains(item.GetComponent<LevelInteracte>().NBT))
            {
                item.GetComponent<LevelInteracte>().InteractOn();
            }
        }
    }
}
