using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    CharacterController2D controller;
    private void Start()
    {
        controller = transform.parent.GetComponent<CharacterController2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        bool wasGrounded = controller.m_Grounded;
        controller.m_Grounded = true;
        if (!wasGrounded)
            controller.OnLandEvent.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        controller.m_Grounded = false;
    }
}
