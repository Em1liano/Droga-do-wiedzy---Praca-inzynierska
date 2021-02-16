using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Zmienne
    [Header("Variables")]
    public float currentHealth;
    public int maxHealth = 100;
    float lastRegen;
    float healthRegentAmount = 1f;
    [SerializeField] float healthRegenSpeed;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        currentHealth = GetComponent<PlayerHealth>().currentHealth;
        // Przypisanie aktualnego zdrowia w zalezności od maksymalnego zdrowia
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Po wczytaniu save automatycznie przypisanie zdrowia do maksymalnej ilości.
        currentHealth = maxHealth;
    }
    // Funkcja zwracająca float, która o daną ilość modyfikuje życie z efektem ubycia nienatychmiastowego
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
    private void Update()
    {
        currentHealth = GetComponent<PlayerHealth>().currentHealth;

        if (maxHealth == 0)
        {
            maxHealth = (int)currentHealth;
        }
        
        // Regeneracja zdrowia
        if (GetComponent<PlayerController>().isAlive == true && currentHealth < maxHealth)
        {
            RegenHealth();
        }
        else
        {
            // Nie regeneruj życia
        }
        
    }

    // Regeneracja zdrowia, po upływie czasu mana zwiększa się o daną ilość
    void RegenHealth()
    {
        if (Time.time - lastRegen > healthRegenSpeed && currentHealth <= maxHealth)
        {
            GetComponent<PlayerHealth>().currentHealth += healthRegentAmount;
            ModifyHealth(healthRegentAmount);
            lastRegen = Time.time;
        }
    }

    public string getCurrentHealth()
    {
        return currentHealth.ToString();
    }
    public string getMaxHealth()
    {
        return maxHealth.ToString();
    }
}
