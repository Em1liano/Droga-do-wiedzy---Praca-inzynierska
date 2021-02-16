using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    // Referencje
    [Header("References")]
    public NavMeshAgent agent;
    public Animator anim;

    // Zmienne
    public bool canMove = true;

    // Update is called once per frame
    void Update()
    {
        // jeżeli gracz posiada ściężke to wykonuje animacje chodzenia
        anim.SetBool("isMoving", agent.hasPath); 
    }
    // funckja zwraca Vector3, czyli pozycje, do którego musi się udać
    public void MoveToPoint(Vector3 pos)
    {
        // jeżeli możę się poruszać to ustala cel na określoną pozycje
        // boolean canMove przydaje się, ponieważ nie zawsze gracz będzie mógł się poruszać tzn może zostać nieuruchomiony lub zabity
        if (canMove)
        {
            agent.SetDestination(pos);
        }
    }
    // funkcja ustala pozycje i porusza się gracza do pozycji
    public void SetPosition(Vector3 pos)
    {
        agent.Warp(pos);
    }
    // funkcja anuluje poruszanie się gracza
    public void StopMoving()
    {
        // sprawdzenie czy gracz istnieje
        if (agent != null)
        {
            // przerwanie poruszania się
            agent.isStopped = true;
            // zresetowanie ścieżki
            agent.ResetPath();
        }
    }
    public void Die()
    {
        // zmienia możliwość poruszania się
        canMove = false;
    }
}
