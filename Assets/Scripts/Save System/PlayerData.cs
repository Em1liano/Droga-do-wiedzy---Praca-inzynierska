using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using TMPro;

[Serializable]
public class PlayerData
{
    // Zmienna string trzymająca nazwę zapisu
    public String Name;

    // Zmienne trzymające pozycje
    public float posX;
    public float posY;
    public float posZ;

    // Zmienne trzymające poziom i doświadczenie
    public int level;
    public float experience;

    // Zmienne statystyk
    public float damage;
    public int health;
    public float mana;

    // Aktualna misja
    public Quest currentQuest;

    // Aktualny świat
    public int currentWorld;
    //public TextMeshProUGUI titleTextSave;
    //public TextMeshProUGUI descriptionTextSave;
    //public TextMeshProUGUI experienceTextSave;

    // Konstruktor zwracający PlayerController
    public PlayerData(PlayerController p)
    {
        // Przypisanie statystyk, pobiera dane z PlayerControllera
        Name = p.stats.Name;

        posX = p.transform.position.x;
        posY = p.transform.position.y;
        posZ = p.transform.position.z;

        level = p.stats.level;
        experience = p.stats.experience;

        damage = p.stats.damage;
        health = p.GetComponent<PlayerHealth>().maxHealth;
        mana = p.GetComponent<Mana>().maxMana;

        // Przypisanie misji z zapisu
        currentQuest = p.quest;

        // Przypisanie świata z zapisu
        currentWorld = p.stats.currentWorld;
    }
    public PlayerData(string name)
    {
        // Standordowy PlayerData trzymający statystyki startowe
        Name = name;

        posX = -100;
        posY = 0;
        posZ = 318;

        level = 1;
        experience = 0;

        damage = 40;
        health = 200;
        mana = 300;

        currentQuest = null;
        currentWorld = 1;
    }
}
