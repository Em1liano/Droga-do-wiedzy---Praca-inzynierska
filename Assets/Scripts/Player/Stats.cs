using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // Referencje
    [Header("References")]
    public Text levelText;
    public Image experienceImage;

    // Zmienne
    [Header("Variables")]
    public string Name = "player";
    public float damage = 40;
    public int level = 1;
    public float experience;
    public float experienceToNextLevel;

    // Aktualny świat
    public int currentWorld;

    // Zmienne progresujące
    [Header("Progress Level Variables")]
    public float damagePerLevel = 1.2f;
    public int healthPerLevel = 50;
    public int manaPerLevel = 25;

    private void Update()
    {
        // Ustawienie level w UI co klatkę
        SetLevel();
        // Obliczanie co klatę aktualnego doświadczenia w UI
        GetExperienceFillAmount();

        // Testowanie
        if (Input.GetKeyDown(KeyCode.X))
        {
            RankUp();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            AddExperience(50);
        }
    }

    // Ustawienie levela
    public void SetLevel()
    {
        levelText.text = level.ToString();
    }

    // Poziom w górę
    public void RankUp()
    {
        level++;

        // Instrukcja, która co level ulepsza statystyki gracza
        switch (level)
        {
            case 2:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 2");
                break;
            case 3:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 3");
                break;
            case 4:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 4");
                break;
            case 5:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 5");
                break;
            case 6:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 6");
                break;
            case 7:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 7");
                break;
            case 8:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 8");
                break;
            case 9:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 9");
                break;
            case 10:
                GetComponent<PlayerHealth>().maxHealth += healthPerLevel;
                GetComponent<Mana>().maxMana += manaPerLevel;
                damage *= damagePerLevel;
                Debug.Log("Add 50 hp LEVEL 10");
                break;
        }
    }

    // Funckja dodająca doświadczenie dla gracza
    public void AddExperience(int amount)
    {
        experience += amount;

        // Jeżeli aktualne doświadczenie >= potrzebnemu dośw do następnego levelu to wywołuje funkcje zwiększającą level
        while (experience >= experienceToNextLevel)
        {
            RankUp();
            experience -= experienceToNextLevel;

            experienceToNextLevel *= 1.01f;
        }
    }
    // Zwraca aktualny level
    public int GetLevelNumber()
    {
        return level;
    }
    // Zwraca aktualną wartość doświadczenia
    public double GetExperience()
    {
        return experience;
    }
    // Zwraca potrzebną ilość doświadczenia do następnego poziomu
    public double GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }
    // Ustala w UI aktualna wartość doświadczenia w zależności od potrzebnego doświadczenia
    public void GetExperienceFillAmount()
    {
        experienceImage.fillAmount = experience / experienceToNextLevel;
    }
}
