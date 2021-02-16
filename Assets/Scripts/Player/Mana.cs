using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Zmienne
    [Header("Variables")]
    public float currentMana;
    public float maxMana = 300f;
    float lastRegen;
    float manaRegentAmount = 1f;
    [SerializeField] float ManaRegenSpeed;

    // Event, który daje efekt polepszający wyświetlanie many
    public event Action<float> OnManaPctChanged = delegate { };

    // Przypisanie aktualnej wartości many w zależności od wartości maksymalnej many
    private void OnEnable()
    {
        currentMana = maxMana;
    }
    // Funkcja zwracająca float, która o daną ilość modyfikuje mane z efektem
    public void ModifyMana(float amount)
    {
        currentMana += amount;

        float currentHealthPct = (float)currentMana / (float)maxMana;
        OnManaPctChanged(currentHealthPct);
    }

    // Update is called once per frame
    void Update()
    {
        // Regeneracja many
        if (GetComponent<PlayerController>().isAlive == true && currentMana < maxMana)
        {
            RegenMana();
        }
        else
        {
            // Nie regeneruj many
        }
        if (maxMana == 0)
        {
            maxMana = currentMana;
        }
    }

    // Regeneracja many, po upływie czasu mana zwiększa się o daną ilość
    void RegenMana()
    {
        if (Time.time - lastRegen > ManaRegenSpeed && currentMana <= maxMana)
        {
            currentMana += manaRegentAmount;
            ModifyMana(manaRegentAmount);
            lastRegen = Time.time;
        }
    }
    public string getCurrentMana()
    {
        return currentMana.ToString();
    }
    public string getMaxMana()
    {
        return maxMana.ToString();
    }
}
