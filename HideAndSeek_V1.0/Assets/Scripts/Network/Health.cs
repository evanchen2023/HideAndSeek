using System;
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
    private NetworkSpawner networkSpawner;
    private GameObject networkManager;

    private void Start()
    {
        networkManager = GameObject.FindWithTag("NetworkSpawner");
        networkSpawner = networkManager.GetComponent<NetworkSpawner>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            networkSpawner.KillPlayer(Object);
        }

        healthBar.SetHealth(currentHealth);
    }
}