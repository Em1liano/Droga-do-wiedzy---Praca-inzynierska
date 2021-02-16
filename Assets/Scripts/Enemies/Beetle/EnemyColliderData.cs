using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderData : MonoBehaviour
{
    // Referencje
    public EnemyMeleeController controller;
    // Zmienne
    private bool isPlayerInRange = false;

    // Funkcja zwracająca boolean czy gracz jest w zasięgu
    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Po wejściu gracza w trigger przypisuje isPlayerInRange na prawdę i ustawia cel na gracza
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerInRange = true;
            controller.SetTarget(other.gameObject.GetComponent<PlayerController>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Po wyjściu z triggera przypisuje isPlayerInRange na fałsz i resetuje cel
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerInRange = false;
            controller.ResetTarget();
        }
    }
}
