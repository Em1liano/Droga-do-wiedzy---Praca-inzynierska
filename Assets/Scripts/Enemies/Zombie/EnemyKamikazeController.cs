using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Maszyny stanów
public enum ZombieState
{
    Roam,
    Chase,
    Waiting,
    Explosion
}

public class EnemyKamikazeController : MonoBehaviour
{
    // Scriptable object
    [Header("Scriptable object")]
    public EnemyType enemy;

    // Referencje
    [Header("References")]
    public AgentMovement movement;
    public PlayerController target;
    private Animator anim;
    public EnemyZombieColliderData colliderData;
    public GameObject exploadFX;

    // Zmienne do roamingu
    [Header("Roaming")]
    public float RoamDelay = 5f;
    public float RoamDistanceX = 5f;
    public float RoamDistanceZ = 5f;
    private float RoamTimer = 0;
    private Vector3 startingPos;
    public float moveDistance = 8f;

    // Statystyki
    [Header("Stats Enemy")]
    public bool isAlive = true;
    public bool isAttacking = false;
    public bool isExploading = false;
    public bool isExploaded = false;

    [Header("Expload")]
    public float timeToDestroy = 2f;

    // Aktualny stan maszynowy
    [SerializeField] private ZombieState _currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Przypisanie prędkości przeciwnika używając scriptable object
        GetComponent<AgentMovement>().agent.speed = enemy.speed;
        anim = GetComponent<Animator>();
        // Przypisanie pozycji początkowej pozycji, w której na początku ustawi się przeciwnika
        startingPos = transform.position;
    }
    // Ustawienie celu, gdzie zwracaną wartością jest odwołanie do gracza
    public void SetTarget(PlayerController player)
    {
        target = player;
    }
    // Zresetowanie celu
    public void ResetTarget()
    {
        target = null;
    }
    // Funkcja odlicza określony czas a następnie szuka i podąża do określonej pozycji
    private void Roam()
    {
        RoamTimer -= Time.deltaTime;
        if (RoamTimer <= 0f)
        {
            RoamTimer = RoamDelay;

            float newPosX = Random.Range(-RoamDistanceX, RoamDistanceX);
            float newPosZ = Random.Range(-RoamDistanceZ, RoamDistanceZ);

            Vector3 newPosition = new Vector3(startingPos.x + newPosX, 0, startingPos.z + newPosZ);
            movement.MoveToPoint(newPosition);
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            // Stan Roam, wywołuje funckje Roam() jeśli gracz nie jest w zasięgu. Jeżeli jest to zmienia stan na Chase
            case ZombieState.Roam:
                {
                    if (isAlive == true)
                    {
                        if (!colliderData.IsPlayerInRange())
                        {
                            Roam();
                        }
                        else
                        {
                            _currentState = ZombieState.Chase;
                        }
                    }
                    break;
                }
            // Stan Chase oblicza dystans pomiędzy sobą a graczem, dzięki któremu wie kiedy się zatrzymać przed graczem
            case ZombieState.Chase:
                {
                    if (isAlive == true)
                    {
                        if (colliderData.IsPlayerInRange())
                        {
                            float distance = Vector3.Distance(transform.position, target.transform.position);

                            if (distance >= moveDistance)
                            {
                                // poruszanie się w stronę gracza i resetowanie zmiennych do wybuchu aby nie otrzymać niespodziewanych błędów
                                MoveToTarget();
                                isExploading = false;
                                timeToDestroy = 2f;
                                StopKamikaze();
                            }
                            else
                            {
                                // Jeżeli jest blisko zmienia stan na Waiting
                                _currentState = ZombieState.Waiting;
                            }
                        }
                        else
                        {
                            // jeśli gracz wyjdzie z zasięgu to powrót do stanu Roam
                            _currentState = ZombieState.Roam;
                        }
                    }
                    break;
                }
                // Stan Waiting posiada timer, który odlicza określone sekundy i następnie przechodzi do ostatecznego stanu Explosion
            case ZombieState.Waiting:
                {
                    if (isAlive == true)
                    {
                        movement.StopMoving();
                        FaceToTarget();

                        timeToDestroy -= Time.deltaTime;
                        isExploading = true;
                        Debug.Log(timeToDestroy);

                        if (timeToDestroy <= 0 && isExploading == true)
                        {
                            _currentState = ZombieState.Explosion;
                        }
                    }
                    break;
                }
                // Stan Explosion wywołuje funkcje StartKamikaze
            case ZombieState.Explosion:
                {
                    if (isAlive == true)
                    {
                        StartKamikaze();
                    }
                    else
                    {
                        return;
                    }
                    break;
                }
        }
    }
    private void FaceToTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.75f);
    }
    public void MoveToTarget()
    {
        if (target != null)
        {
            movement.MoveToPoint(target.transform.position);
        }
    }
    
    public bool GetHit(float value)
    {
        GetComponent<EnemyHealth>().ModifyHealth(-value);
        SoundManager.PlaySound(SoundManager.Sound.ZombieGetHit, transform.position);
        if (GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            Die();
            StartCoroutine(GetComponent<EnemyHealth>().TurnOffCanvas());
            return true;
        }
        return false;
    }
    public void Hit()
    {
        if (target.dashing == false)
        {
            if (target != null)
            {
                if (target.GetHit(enemy.damage))
                {
                    StopKamikaze();
                }
            }
        }
        else
        {
            return;
        }
    }
    public void StartKamikaze()
    {
        if (!isExploaded)
        {
            // jeżeli powyższa zmienna to prawda to wywołuje wybuch, tzn efekt particle system w określonym miejscu
            isExploaded = true;
            // exploadFX posiada skrypt, który dodatkowo sprawdza czy gracz zdążył uciec z zasięgu wybuchu
            SoundManager.PlaySound(SoundManager.Sound.ZombieExpload, transform.position);
            Instantiate(exploadFX, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            // po wybuchnięciu niszczy obiekt Zombie
            Destroy(gameObject, 0.1f);
        }
    }
    // Anulowanie wybuchu
    public void StopKamikaze()
    {
        Debug.Log("Reset expload");
        if (isExploaded)
        {      
            isExploaded = false;
        }
    }
    public void Die()
    {
        isAlive = false;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        // TODO stop exploading
        StopKamikaze();
        GetComponent<CapsuleCollider>().enabled = false;
        anim.SetBool("isAlive", isAlive);

        //exp
        FindObjectOfType<Stats>().AddExperience(enemy.expPerKill);
        StartCoroutine(HideShowSkin(false));
    }
    IEnumerator HideShowSkin(bool state)
    {
        yield return new WaitForSeconds(10f);
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer meshes in skinMeshList)
        {
            meshes.enabled = state;
        }
    }
}
