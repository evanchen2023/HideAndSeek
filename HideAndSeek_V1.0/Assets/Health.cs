using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine.UI;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public const int MAX_HEALTH = 100;
    public int currentHealth = MAX_HEALTH;
    public HealthBar healthBar;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        }

        healthBar.SetHealth(currentHealth);
    }
}