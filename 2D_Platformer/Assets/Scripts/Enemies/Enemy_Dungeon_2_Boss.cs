using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dungeon_2_Boss : Enemy
{
    bool wasDamaged = false;
    float oldTime = 0;
    float oldDeltaTime = 0;
    public Collider2D headCol;
    public Collider2D leftArmCol;
    public Collider2D rightArmCol;

    protected override void onTouch(GameObject player, Collider2D col)
    {
        if (lives <= 0)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (item.GetComponent<AlphaNBTTag>().NBT == 180)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
                if (item.GetComponent<AlphaNBTTag>().NBT == 170)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
                if (item.GetComponent<AlphaNBTTag>().NBT == 160)
                {
                    item.GetComponent<LevelInteracte>().InteractOff();
                }
            }
            if(GameManager.instance.currentQuests.Exists((x)=>x.name == "A Heart of Darkness"))
            {
                GameManager.instance.currentQuests.Find((x) => x.name == "A Heart of Darkness").CompleteQuestStep();
                GameManager.instance.currentQuests.Find((x) => x.name == "A Heart of Darkness").isFinished();
            }
            else
            {
                if(!GameManager.instance.currentQuests.Exists((x) => x.name == "A Heart of Darkness" || x.name == "Enhanced Heart of Darkness" || x.name == "Awakend Heart of Darkness"))
                    GameManager.instance.AddQuest("A Mysterious Heart");
            }
            Invoke("Finish", 1f);
            Time.timeScale *= 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.2f;
            //GetComponent<Animator>().SetTrigger("Death");
        }
        else
        {
            if (!wasDamaged)
            {
                wasDamaged = true;
                Debug.Log("Live lost!");
                lives--;
                if(GameObject.FindGameObjectWithTag("Player") != null)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().mushRoomTime = player.GetComponent<PlayerStats>().mushroomEffect.Time;
                if (GameObject.FindGameObjectWithTag("Player_2") != null)
                    GameObject.FindGameObjectWithTag("Player_2").GetComponent<PlayerStats>().mushRoomTime = player.GetComponent<PlayerStats>().mushroomEffect.Time;
                Vector2 vel = (player.transform.position - transform.position).normalized * 25f;
                player.GetComponent<Rigidbody2D>().velocity = vel;
                StartCoroutine(selectOtherLive());
            }
        }
    }

    IEnumerator selectOtherLive()
    {
        yield return new WaitForSeconds(1f);
        if (lives == 2)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (item.GetComponent<AlphaNBTTag>().NBT == 170)
                {
                    item.GetComponent<LevelInteracte>().InteractOn();
                }
            }
        }
        if (lives == 1)
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
            {
                if (item.GetComponent<AlphaNBTTag>().NBT == 160)
                {
                    item.GetComponent<LevelInteracte>().InteractOn();
                }
            }
        }
        wasDamaged = false;
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
        foreach (var item in GameObject.FindGameObjectsWithTag("LeverInteract"))
        {
            if (item.GetComponent<AlphaNBTTag>().NBT == 180)
            {
                item.GetComponent<LevelInteracte>().InteractOff();
            }
            if (item.GetComponent<AlphaNBTTag>().NBT == 170)
            {
                item.GetComponent<LevelInteracte>().InteractOff();
            }
            if (item.GetComponent<AlphaNBTTag>().NBT == 160)
            {
                item.GetComponent<LevelInteracte>().InteractOff();
            }
        }
        if (gb.transform.tag == "Player" || gb.transform.tag == "Player_2")
            gb.transform.GetComponent<PlayerStats>().Die();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.GetComponent<PlayerStats>().Die();
        StopAllCoroutines();
        wasDamaged = false;
        lives = 3;
    }

    public override void Start()
    {
        base.Start();
        oldDeltaTime = Time.fixedDeltaTime;
        oldTime = Time.timeScale;
    }

    protected override void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<PlayerStats>().isDead)
            {
                ResetOnDeath(player);
            }
        }
    }

    GameObject player = null;

    
    protected override void Move()
    {

    }

    int lives = 3;
}
