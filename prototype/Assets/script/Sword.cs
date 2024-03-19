using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage the sword deals

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the boss
        if (other.CompareTag("Enemy"))
        {
            // Get the boss component and apply damage to it
            boss BossScript = other.GetComponent<boss>();
            if (BossScript != null)
            {
                BossScript.TakeDamage(damageAmount);
            }
        }
    }
}
