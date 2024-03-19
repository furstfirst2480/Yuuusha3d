using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe : MonoBehaviour
{
    public float damageAmount = 10f; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the boss
        if (other.CompareTag("Player"))
        {
            // Get the boss component and apply damage to it
            ThePlayer PlayerScript = other.GetComponent<ThePlayer>();
            if (PlayerScript != null)
            {
                if (PlayerScript.paralyze == false)
                { 
                PlayerScript.TakeDamage(damageAmount);
                }
            }
        }
    }
}
