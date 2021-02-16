using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Stany maszynowe
public enum BeetleState
{
    Roam,
    Chase,
    Attack
}

public class EnemyMeleeController : MonoBehaviour
{
    // Scriptable object
    [Header("Scriptable object")]
    public EnemyType enemy;
    
    // Referencje
    [Header("References")]
    public AgentMovement movement;
    public PlayerController target;
    private Animator anim;
    public EnemyColliderData colliderData;
    
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

    // Aktualny stan maszynowy
    [SerializeField] private BeetleState _currentState;


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
        // Maszyny stanó
        switch (_currentState)
        {
            // Stan Roam, wywołuje funckje Roam() jeśli gracz nie jest w zasięgu. Jeżeli jest to zmienia stan na Chase
            case BeetleState.Roam:
                {
                    if (isAlive == true)
                    {
                        if (!colliderData.IsPlayerInRange())
                        {
                            Roam();
                        }
                        else
                        {
                            _currentState = BeetleState.Chase;
                        }
                    }
                    break;
                }
            // Stan Chase oblicza dystans pomiędzy sobą a graczem, dzięki któremu wie kiedy się zatrzymać przed graczem i zacząć strzelać
            case BeetleState.Chase:
                {
                    if (isAlive == true)
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);

                        if (distance >= moveDistance)
                        {
                            MoveToTarget();
                            StopAttacking();
                        }
                        else
                        {
                            // Jeżeli jest blisko zmienia stan na Shoot
                            _currentState = BeetleState.Attack;
                        }
                    }
                    break;
                }
                // Stan Attack sprawdza czy gracz istnieje, aby nie wyskakiwały niespodziewane błędy
                // oblicza dystans do gracza i porusza się w jego kierunku i zatrzymuje w odpowiednim miejscu
                // następnie zaczyna atakować natomiast przestaje atakować, jeśli gracz ucieknie albo zginie
            case BeetleState.Attack:
                {
                    if (target != null && isAlive == true)
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);

                        if (distance <= 3f && isAlive == true)
                        {
                            movement.StopMoving();
                            StartAttacking();
                        }
                        else if (isAlive == true)
                        {
                            MoveToTarget();
                            StopAttacking();
                        }
                    }
                    if (!colliderData.IsPlayerInRange())
                    {
                        StopAttacking();
                        _currentState = BeetleState.Roam;
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
    public void StartAttacking()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", isAttacking);
        }
    }
    public void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
        }
    }
    public bool GetHit(float value)
    {
        GetComponent<EnemyHealth>().ModifyHealth(-value);
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
                    StopAttacking();
                }
            }
        }
        else
        {
            return;
            // Can't hit
        }
    }
    public void HitSFX()
    {
        SoundManager.PlaySound(SoundManager.Sound.BeetleHit, transform.position);
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
    public void Die()
    {
        SoundManager.PlaySound(SoundManager.Sound.BeetleDead, transform.position);
        isAlive = false;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        StopAttacking();
        GetComponent<CapsuleCollider>().enabled = false;
        anim.SetBool("isAlive", isAlive);

        //exp
        FindObjectOfType<Stats>().AddExperience(enemy.expPerKill);

        if (target.quest.isActive)
        {
            target.quest.goal.EnemyKilled();
            if (target.quest.goal.IsReached())
            {
                FindObjectOfType<Stats>().AddExperience(target.quest.experienceReward);
                target.quest.Complete();
                PlayerPrefs.SetInt("First Mission Status", 2);
                FindObjectOfType<QuestGiver>().isMissionComplited = true;
            }
        }

        StartCoroutine(HideShowSkin(false));
    }
    
}
