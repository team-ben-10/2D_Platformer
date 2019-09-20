using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreneGameplay : MonoBehaviour
{

    float timer = 0;
    float currentTimer;
    Animator controller;

    void Start()
    {
        currentTimer = timer;
        controller = GetComponent<Animator>();
    }

    bool isFadedIn = true;

    void Update()
    {
        if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Crouch") || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().horizontalMove != 0)
        {
            currentTimer = 3;
            controller.SetBool("FadeIn", false);
            controller.SetBool("FadeOut", true);
            isFadedIn = false;
        }

        if(currentTimer <= 0)
        {
            FadeIn();
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }
    }

    void FadeIn()
    {
        if (!isFadedIn)
        {
            controller.SetBool("FadeIn", true);
            controller.SetBool("FadeOut", false);
            isFadedIn = true;
        }
    }
}
