using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedAbility : Ability
{
    private void Start()
    {
        oldRunSpeed = GetComponent<PlayerMovement>().runSpeed;
        oldjumpForce = GetComponent<CharacterController2D>().m_JumpForce;
    }

    float runTime;
    bool spawnParticle;
    float oldRunSpeed;
    float oldjumpForce;
    GameObject particle;
    
    private void Update()
    {
        if (GetComponent<PlayerStats>().isDead)
            runTime = 0;
        if(runTime == 0)
        {
            GetComponent<PlayerMovement>().runSpeed = oldRunSpeed;
            GetComponent<CharacterController2D>().m_JumpForce = oldjumpForce;
            if (particle != null)
            {
                particle.GetComponent<ParticleSystem>().loop = false;
                Destroy(particle, 2f);
            }
        }

        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            runTime++;
        }
        else
        {
            runTime = 0;
            spawnParticle = false;
        }

        if(runTime >= 120)
        {
            if (!spawnParticle)
            {
                spawnParticle = true;
                particle = Instantiate(objsToSpawn[0], transform.position, Quaternion.identity, transform);
                GetComponent<PlayerMovement>().runSpeed = oldRunSpeed * 1.5f;
                GetComponent<CharacterController2D>().m_JumpForce = oldjumpForce * 1.25f;
            }
        }
    }
}
