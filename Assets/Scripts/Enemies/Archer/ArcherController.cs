using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Określone stany
public enum ArcherState
{
    Roam,
    Chase,
    Shoot,
    AttackMelee
}

public class ArcherController : MonoBehaviour
{
    // Scriptable object
    [Header("Scriptable object")]
    public EnemyType enemy;

    // Referencje
    [Header("References")]
    public AgentMovement movement;
    public PlayerController target;
    private Animator anim;
    public ArcherColliderData colliderData;
    // Referencje do strzelania
    [Header("Projectile")]
    public GameObject arrowPrefab;
    public GameObject gunPos;

    // Zmienne do roamingu
    [Header("Roaming")]
    // Roaming
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
    public bool isShooting = false;
    public bool isAttackingMelee = false;
    public float healthPerc;


    // Aktualny stan maszynowy
    [SerializeField] private ArcherState _currentState;

    private void Start()
    {
        // Przypisanie prędkości przeciwnika z scriptable object
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
        _currentState = ArcherState.Roam;
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

    private void Update()
    {
        // Maszyny stanów
        switch (_currentState)
        {
            // Stan Roam, wywołuje funckje Roam() jeśli gracz nie jest w zasięgu. Jeżeli jest to zmienia stan na Chase
            case ArcherState.Roam:
                {
                    if (!colliderData.IsPlayerInRange())
                    {
                        Roam();
                    }
                    else
                    {
                        _currentState = ArcherState.Chase;
                    }
                    break;
                }
            // Stan Chase oblicza dystans pomiędzy sobą a graczem, dzięki któremu wie kiedy się zatrzymać przed graczem i zacząć strzelać
            case ArcherState.Chase:
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);

                    if (distance >= moveDistance)
                    {
                        MoveToTarget();
                        StopShooting();
                    }
                    else
                    {
                        // Jeżeli jest blisko zmienia stan na Shoot
                        _currentState = ArcherState.Shoot;
                    }
                    break;
                }
            // Stan Shoot wywołuje zaczyna strzelać oraz ustawia się w kierunku gracza, jeśli jest w zasięgu
            case ArcherState.Shoot:
                {
                    if (colliderData.IsPlayerInRange())
                    {
                        movement.StopMoving();
                        StartShooting();
                        FaceToTarget();
                    }
                    else
                    {
                        // Jeżeli nie jest to zmienia stan na Roam
                        StopShooting();
                        _currentState = ArcherState.Roam;
                    }

                    // Warunek jeśli gracz jest w zasięgu oraz jeśli ma mniej niz 50% życia to zmienia stan na atak wręcz
                    if (colliderData.IsPlayerInRange())
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);

                        if (distance >= 1f && healthPerc <= 50)
                        {
                            StopShooting();
                            _currentState = ArcherState.AttackMelee;
                        }
                    }
                    break;
                }
            // Stan AttackMelee oblicza i podbiega do gracza i zaczyna atak wręcz
            case ArcherState.AttackMelee:
                {
                    if (target != null)
                    {
                        float distance = Vector3.Distance(transform.position, target.transform.position);

                        if (distance <= 3f && isAlive == true)
                        {
                            movement.StopMoving();
                            StartAttackMelee();
                        }
                        else if (isAlive == true)
                        {
                            MoveToTarget();
                            StopAttackingMelee();
                        }
                    }
                    break;
                }
        }
        // Wywołanie co klatkę sprawdzania % życia
        CalculatePercOfHealth();
    }
    // Obliczanie % życia przeciwnika
    private void CalculatePercOfHealth()
    {
        healthPerc = ((GetComponent<EnemyHealth>().currentHealth / enemy.maxHealth) * 100);
    }

    // Funkcja oblicza rotację jaką musi wykonać, aby być równo obrócona w stronę gracza
    private void FaceToTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.75f);
    }
    // Funkcja sprawdza czy cel istnieje i następnie przesuwa się do określonej pozycji
    public void MoveToTarget()
    {
        if (target != null)
        {
            movement.MoveToPoint(target.transform.position);
        }
    }
    // Funkcja zaczyna wykonywać animacje strzelania
    public void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            anim.SetBool("isShooting", isShooting);
        }
    }
    public void BowSFX()
    {
        SoundManager.PlaySound(SoundManager.Sound.ArcherBow, transform.position);
    }
    public void MeleeSFX()
    {
        SoundManager.PlaySound(SoundManager.Sound.ArcherMelee, transform.position);
    }
    // Funkcja zaczyna animacje ataku wręcz
    public void StartAttackMelee()
    {
        if (!isAttackingMelee)
        {
            isAttackingMelee = true;
            anim.SetBool("isAttackMelee", isAttackingMelee);
        }
    }
    // Funkcja przestaje animować atak wręcz
    public void StopAttackingMelee()
    {
        isAttackingMelee = false;
        anim.SetBool("isAttackMelee", false);
    }
    // Funkcja przestaje animować strzał
    public void StopShooting()
    {
        isShooting = false;
        anim.SetBool("isShooting", false);
    }
    // Funkcja zwraca prawdę lub fałsz i wartość float jaką otrzymuje obrażem od gracza
    public bool GetHit(float value)
    {
        GetComponent<EnemyHealth>().ModifyHealth(-value);
        // Jeżeli gracz ma mniej lub równo 0 życia to wykonuje się funkcja Die, również wyłącza się Canvas pokazujący życie
        if (GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            Die();
            StartCoroutine(GetComponent<EnemyHealth>().TurnOffCanvas());
            return true;
        }
        return false;
    }
    // Funkcja potrzebna do zadawania obrażeń z łuku
    public void ArcherHit()
    {
        if (target.dashing == false)
        {
            if (target != null)
            {
                if (target.GetHit(enemy.damage))
                {
                    StopShooting();
                }
            }
        }
        else
        {
            return;
        }
    }
    // Funkcja wyłączająca funkcjonalność przeciwnika
    public void Die()
    {
        SoundManager.PlaySound(SoundManager.Sound.ArcherDead, transform.position);
        isAlive = false;     
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        StopShooting();
        GetComponent<CapsuleCollider>().enabled = false;
        anim.SetBool("isAlive", isAlive);

        //exp
        FindObjectOfType<Stats>().AddExperience(enemy.expPerKill);
        StartCoroutine(HideShowSkin(false));
    }

    // Tworzenie strzały w określonej pozycji gunPos
    public void SpawnArrow()
    {
        Instantiate(arrowPrefab, gunPos.transform.position, Quaternion.identity);
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
