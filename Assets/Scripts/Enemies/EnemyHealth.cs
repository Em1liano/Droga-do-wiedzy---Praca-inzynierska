using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour
{
    // Referencja do EnemyType
    public EnemyType enemy;

    // Referencja do Canvasu
    public Canvas healthCanvas;

    // Zmienne
    public float currentHealth;

    // Event sprawdzający czy życie ubyło
    public event Action<float> OnHealthPctChanged = delegate { };
    
    // Przypisanie aktualnego życia do scriptable object enemy w zależności od typu
    private void OnEnable()
    {
        currentHealth = enemy.maxHealth;
    }
    // Funkcja zwracająca float, która o daną ilość modyfikuje życie z efektem ubycia nienatychmiastowego
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)enemy.maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
    // Coroutine wyłącza canvas z życiem po upłynięniu 1.5 sekundy
    public IEnumerator TurnOffCanvas()
    {
        yield return new WaitForSeconds(1.5f);
        healthCanvas.enabled = false;
    }
    // Coroutina włącza canvas z życiem
    private IEnumerator TurnOnCanvas()
    {
        healthCanvas.enabled = true;
        yield return null;
    }
}
