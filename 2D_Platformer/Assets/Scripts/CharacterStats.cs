using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public virtual void Die()
    {
        Debug.Log(transform.name + "died!");
    }
    

}
