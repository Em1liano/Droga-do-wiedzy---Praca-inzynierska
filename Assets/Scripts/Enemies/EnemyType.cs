using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyType : ScriptableObject
{
    [Header("Description")]
    // String nadający nazwę przeciwnikowi
    public new string name;
    // String opisujący przeciwnika
    public string description;

    [Header("Health")]
    // Maksymalne życie typu przeciwnika oraz doświadczenie za zabicie jednostki
    public int maxHealth = 100;
    public int expPerKill = 25;

    [Header("Attacking")]
    // Obrażenia jednostki
    public float damage;
    // TODO defence w stylu % zmniejszający obrażenia gracza
    public int defence;
    // Prędkość jednostki
    public float speed;
}
