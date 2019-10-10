using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{

    public Vector3 lastCheckPoint;
    [HideInInspector] public float mushRoomTime;
    float maxMushRoomTime;
    bool wasOnIce;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Icy" && GetComponent<CharacterController2D>().m_Grounded)
        {
            GetComponent<CharacterController2D>().m_MovementSmoothing = 2f;
        }
        else
        {
            GetComponent<CharacterController2D>().m_MovementSmoothing = 0.05f;
        }
    }

    private void Update()
    {
        if (mushRoomTime <= 0 && mushroomEffect != null)
        {
            mushroomEffect.OnEnd(gameObject, mushRoomTime, -1);
            mushroomEffect = null;
            mushRoomTime = 0;
        }
        if (mushRoomTime > 0 && mushroomEffect != null)
        {
            mushroomEffect.OnDraw(gameObject, mushRoomTime);
            mushRoomTime -= Time.deltaTime;
            GameManager.instance.MushroomFillTime.fillAmount = mushRoomTime / maxMushRoomTime;
        }
        if (transform.position.y <= 0)
        {
            Die();
        }
    }

    public bool isDead = false;

    public void ResetMushroomEffect()
    {
        if (mushroomEffect != null)
        {
            mushroomEffect.OnEnd(gameObject, mushRoomTime, -1);
            mushroomEffect = null;
            mushRoomTime = 0;
            GameManager.instance.MushroomFillTime.fillAmount = 0;
        }
    }

    public MushroomEffect mushroomEffect;

    IEnumerator CameraUpdate()
    {
        var gb = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>();
        while (gb.targets.Count <= 0) { yield return null; }
        if (!GetComponent<PlayerMovement>().isNotMain)
            GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets[0] = transform;
    }

    private void Start()
    {
        lastCheckPoint = transform.position;
        StartCoroutine(CameraUpdate());
        StartCoroutine(waitForPlayerSkin());
        GameObject.Find("Back").GetComponent<Scrolling>().ScrollRight();
    }

    public IEnumerator waitForPlayerSkin()
    {
        yield return null;
        GameManager.instance.UpdatePlayer();
    }

    public void PickupItem(MushroomEffect effect)
    {
        if (mushroomEffect != null)
        {
            mushroomEffect.OnEnd(gameObject, mushRoomTime, (effect is VisInstaKillMushroomEffect) ? (effect.GetComponent<AlphaNBTTag>() != null ? effect.GetComponent<AlphaNBTTag>().NBT: -1) : -1);
        }
        effect.OnStart(gameObject);
        mushroomEffect = effect;
        mushRoomTime = mushroomEffect.Time * 60 * Time.deltaTime;
        maxMushRoomTime = mushRoomTime;
    }

    public override void Die()
    {
        if (GetComponent<PlayerMovement>().isNotMain && !isDead)
        {
            GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Remove(transform);
            isDead = true;
            GetComponent<Animator>().SetBool("Dead", true);
            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().hasSpawnedSecondPlayer = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<PlayerMovement>().enabled = false;
            if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().isDead)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().Die();
            StartCoroutine(waitForDeath());
            return;
        }
        if (!isDead)
        {
            isDead = true;
            GameManager.instance.deaths++;
            if (mushroomEffect != null)
            {
                mushroomEffect.OnEnd(gameObject, mushRoomTime,-1);
                mushroomEffect = null;
                mushRoomTime = 0;
                GameManager.instance.MushroomFillTime.fillAmount = 0;
            }
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Animator>().SetBool("Dead", true);
            GetComponent<Animator>().SetBool("Crouch", false);
            GetComponent<Animator>().SetBool("Jump", false);
            GetComponent<PlayerMovement>().PlayerSpawnIndex = 2;
            /*if (GetComponent<PlayerMovement>().hasSpawnedSecondPlayer)
            {
                
                
            }*/
            for (int i = 2; i <= GetComponent<PlayerMovement>().MaxPlayerAtATime; i++)
            {
                var gb = GameObject.FindGameObjectWithTag("Player_" + i);
                if(gb != null)
                    gb.GetComponent<PlayerStats>().Die();
            }
            //GetComponent<PlayerMovement>().hasSpawnedSecondPlayer = false;
            /*if (lastCheckPoint != null)
            {*/
            StartCoroutine(waitForDelayDeath2());
            /*}
            else
            {
                GameManager.instance.levelLoader.CamFollowPlayer = false;
                GameManager.instance.levelLoader.cam.SetFollow(null);
                StartCoroutine(waitForReload());
            }*/
        }
    }

    IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetBool("Dead", false);
        Destroy(gameObject);
    }

    /*IEnumerator waitForReload()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.Restart(false);
        GameManager.instance.levelLoader.CamFollowPlayer = true;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Animator>().SetBool("Dead", false);
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isDead = false;
    }*/

    IEnumerator waitForDelayDeath2()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.levelLoader.LoadEntityObjects();
        transform.position = lastCheckPoint + new Vector3(0, 0.5f, 0);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Animator>().SetBool("Dead", false);
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Clear();
        GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CameraMovement>().targets.Add(transform);
        isDead = false;
        GameManager.instance.UpdatePlayer();
    }

}
