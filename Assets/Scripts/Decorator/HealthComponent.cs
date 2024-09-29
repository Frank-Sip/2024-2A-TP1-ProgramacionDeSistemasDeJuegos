using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void HealToMax()
    {
        currentHealth = maxHealth;
    }

    private void Death()
    {
        var deadObject = GetComponent<IDeathLogic>();

        if (deadObject != null)
        {
            deadObject.Die();
        }
    }
}
