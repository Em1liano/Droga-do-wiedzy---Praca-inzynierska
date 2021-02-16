using Assets.Scripts.Enemies.Crab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Referencje
    [Header("References")]
    public AgentMovement movement;
    public GameObject playerUICanvas;
    private Animator anim;
    private Vector3 destination;
    private GameObject target;
    public Stats stats;
    public TextMeshProUGUI currentHP_Text;
    public TextMeshProUGUI maxHP_Text;
    public TextMeshProUGUI currentMana_Text;
    public TextMeshProUGUI maxMana_Text;

    // Zmienne
    [Header("Variables")]
    public bool isAlive = true;
    private bool isApproachingTarget = false;
    public bool isAttacking = false;
    public float moveDistance = 8f;
    public float bossDistanceStop = 12f;
    public bool isMoving = false;
    public bool missionRef = false;
    public bool missionLevel3Ref = false;

    [Header("Dash")]
    public bool canDash = false;
    public bool isFinishedDash = false;
    public GameObject dashParticle;
    [SerializeField] private float dashCost;
    public bool dashing = false;

    [Header("SpellW")]
    public bool canSpell = false;
    [SerializeField] private float wCost;
    public GameObject swordGlow;

    [Header("SpellE")]
    public bool canSpellE = false;
    [SerializeField] private float eCost;
    public GameObject spellParticle;


    [Header("SwordPassive")]
    public bool canPassive = false;
    [SerializeField] private float passiveCost;
    public bool shouldBuffStats = false;

    [Header("Quest")]
    public Quest quest;

    private List<GameObject> enemies;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SoundManager.Initialize();
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            currentHP_Text.text = GetComponent<PlayerHealth>().getCurrentHealth();
            maxHP_Text.text = GetComponent<PlayerHealth>().getMaxHealth();

            currentMana_Text.text = GetComponent<Mana>().getCurrentMana();
            maxMana_Text.text = GetComponent<Mana>().getMaxMana();


            // Jeżeli wciśnie lewy przycisk myszy poruszanie się do wybranego punktu
            if (Input.GetMouseButton(0))
            {
                Move();
                StopAttacking();

                if (Input.GetKeyDown(KeyCode.Q) && canDash == true && dashCost <= GetComponent<Mana>().currentMana)
                {
                    // Dodać tutorial apropo dash
                    Debug.Log("Start dash");
                    StartCoroutine(Dash());
                    GetComponent<Mana>().ModifyMana(-dashCost);
                }
            }
            // Jeżeli wciśnieta mysz w odpowiedniego przeciwnika
            if (isApproachingTarget)
            {
                MoveToTarget();
            }
            // Jeżeli gracz atakuje to jest obrócony do niego
            if (isAttacking)
            {
                FaceTarget();
            }

            if (anim.GetBool("isMoving") == true)
            {
                isMoving = true;
                canSpell = false;
                SoundManager.PlaySound(SoundManager.Sound.PlayerMove, transform.position);
            }
            else
            {
                isMoving = false;
            }

            if (Input.GetKeyDown(KeyCode.E) && canSpell == true && wCost <= GetComponent<Mana>().currentMana)
            {
                StartCoroutine(SpellE());
                GetComponent<Mana>().ModifyMana(-wCost);
            }

            if (Input.GetKeyDown(KeyCode.W) && passiveCost <= GetComponent<Mana>().currentMana && canPassive == true)
            {
                StartCoroutine(SwordPassive());
            }

            if (quest.isActive)
            {
                FindObjectOfType<GameController>().questCanvas.SetActive(true);

            }
            else FindObjectOfType<GameController>().questCanvas.SetActive(false);
        }
        else
        {
            StartCoroutine(FindObjectOfType<GameController>().ShowEndCanvas());
            return;
        }
    }
    private void Move()
    {
        // Stworzenie ray, który zwraca promień przechodzący z kamery przez punkt na ekranie
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Ustawienie miejsca docelowego
            destination = hit.point;
            // Jeżeli miejsce docelowe ma tag Ground to podążanie tam
            if (hit.collider.transform.tag.Equals("Ground"))
            {
                movement.MoveToPoint(destination);
                isApproachingTarget = false;
            }
            // Jeżeli miejsce docelowe ma tag Enemy to ustawienie celu na ten obiekt
            else if (hit.collider.transform.tag.Equals("Enemy"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Boss"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("NPC"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("SignController"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("MathController"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Level2NPC"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Kristan"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Camilla"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Lumberjack"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("Troll"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
            else if (hit.collider.transform.tag.Equals("GuardL4"))
            {
                target = hit.collider.gameObject;
                isApproachingTarget = true;
            }
        }
    }

    // Funkcja ustalająca pozycje zwracając Vector3
    public void SetPosition(Vector3 pos)
    {
        // Przesuwa gracza do podanego miejsca
        movement.SetPosition(pos);
    }

    // Funkcja przesuwania gracza
    public void MoveToTarget()
    {
        if (isAlive == true)
        {
            if (target != null)
            {
                // Obliczanie dystansu pomiędzy graczem a celem
                float distance = Vector3.Distance(transform.position, target.transform.position);

                // Jeżeli obliczony dystans jest większy od zdefiniowego moveDistans to poruszanie się do określonego celu
                if (distance >= moveDistance && target.gameObject.tag.Equals("Enemy"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= bossDistanceStop && target.gameObject.tag.Equals("Boss"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("NPC"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("Level2NPC"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("SignController"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("MathController"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("Kristan"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("Camilla"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("Lumberjack"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("Troll"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                else if (distance >= moveDistance && target.gameObject.tag.Equals("GuardL4"))
                {
                    movement.MoveToPoint(target.transform.position);
                }
                // Jeżeli nie to wyłączenie poruszania
                else
                {
                    movement.StopMoving();
                    // Jeżeli cel ma tag Enemy to rozpoczenie atakowania
                    if (target.tag.Equals("Enemy"))
                    {
                        StartAttacking();
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Boss"))
                    {
                        StartAttacking();
                        isApproachingTarget = false;
                    }
                    // sprawdzanie czy cel jest NPC
                    else if (target.tag.Equals("NPC"))
                    {
                        if (FindObjectOfType<QuestGiver>().isMissionComplited == true)
                        {
                            missionRef = true;
                        }
                        // jeżeli jest to warunek, aby nie wyświetlał się ponownie canvas z questem po jego aktywacji.
                        QuestState state = FindObjectOfType<QuestGiver>().GetState(QuestState.Dialog);

                        if (state == QuestState.Dialog)
                        {
                            // Otworzenie okienka z informacjami o zadaniu.
                            FindObjectOfType<QuestGiver>().shouldShowDialog = true;
                        }
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("SignController"))
                    {
                        TurnOffCanvas();
                        FindObjectOfType<SignController>().OpenCanvas();
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("MathController"))
                    {
                        TurnOffCanvas();
                        FindObjectOfType<MathController>().TurnOnCanvas();
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Level2NPC"))
                    {
                        TurnOffCanvas();
                        FindObjectOfType<DialogLevel2>().OpenEntryCanvas();
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Kristan"))
                    {
                        if (FindObjectOfType<KristanTask>().isTaskComplited == false)
                        {
                            TurnOffCanvas();
                            FindObjectOfType<KristanTask>().OpenKristanCanvas();
                        }
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Camilla"))
                    {
                        TurnOffCanvas();
                        FindObjectOfType<MathController>().TurnOnCanvas();
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Lumberjack"))
                    {
                        if (FindObjectOfType<QuestGiver>().isLumberMissionComplited == true)
                        {
                            missionLevel3Ref = true;
                        }


                        QuestState state = FindObjectOfType<QuestGiver>().GetState(QuestState.Dialog);

                        if (state == QuestState.Dialog)
                        {
                            // Otworzenie okienka z informacjami o zadaniu.
                            FindObjectOfType<QuestGiver>().shouldShowLumberDialog = true;
                        }
                        isApproachingTarget = false;
                    }
                    else if (target.tag.Equals("Troll"))
                    {
                        if (FindObjectOfType<DialogLevel4>().afterEntryDialog == false)
                        {
                            TurnOffCanvas();
                            FindObjectOfType<DialogLevel4>().OpenEntryCanvas();
                        }
                        else if (FindObjectOfType<DialogLevel4>().afterEntryDialog == true && FindObjectOfType<DialogLevel4>().killedAllEnemies == true)
                        {
                            TurnOffCanvas();
                            FindObjectOfType<DialogLevel4>().OpenEndCanvas();
                        }    
                        isApproachingTarget = false;  
                    }
                    else if (target.tag.Equals("GuardL4"))
                    {
                        TurnOffCanvas();
                        FindObjectOfType<GuardLevel4Dialog>().OpenEntryCanvas();
                        isApproachingTarget = false;
                    }
                }
            }
        }
        else
        {
            Debug.Log("You are dead, can't move");
        }
    }
    // Funkcja wywołuje animacje atakowania
    public void StartAttacking()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", isAttacking);
        }

    }
    // Funkcja przerywa animacje atakowania
    public void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
        }
    }
    // Funkcja oblicza rotację jaką musi wykonać, aby być równo obrócona w stronę gracza
    private void FaceTarget()
    {
        if (target != null)
        {
            Vector3 lookPos = target.transform.position - transform.position;

            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);

        }
        else
        {
            StopAttacking();
            return;
        }
    }
    // Funkcja zwraca prawdę lub fałsz i wartość float jaką otrzymuje obrażem od gracza
    public bool GetHit(float value)
    {
        GetComponent<PlayerHealth>().ModifyHealth(-value);
        SoundManager.PlaySound(SoundManager.Sound.PlayerHit, transform.position);
        if (GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerDead, transform.position);
            isAlive = false;
            StopAttacking();
            anim.SetBool("isAlive", isAlive);
            return true;
        }
        return false;
    }
    // Funkcja dawania obrażeń w zależności od typu przeciwników
    public void Hit()
    {
        if (target != null)
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerAttack, transform.position);


            // Pobieranie komponentu z przeciwników
            EnemyMeleeController enemyMelee = target.GetComponent<EnemyMeleeController>();
            EnemyKamikazeController enemyKamikaze = target.GetComponent<EnemyKamikazeController>();
            ArcherController archer = target.GetComponent<ArcherController>();
            CrabController crab = target.GetComponent<CrabController>();


            // Sprawdzenie jaki jest to typ przeciwnika i następnie zadanie obrażeń
            if (enemyMelee != null)
            {
                if (enemyMelee.GetHit(stats.damage))
                {
                    StopAttacking();
                    target = null;
                }
            }
            else if (enemyKamikaze != null)
            {
                if (enemyKamikaze.GetHit(stats.damage))
                {
                    StopAttacking();
                    target = null;
                }
            }
            else if (archer != null)
            {
                if (archer.GetHit(stats.damage))
                {
                    StopAttacking();
                    target = null;
                }
            }
            else if (crab != null)
            {
                if (crab.GetHit(stats.damage))
                {
                    StopAttacking();
                    target = null;
                }
            }
        }
    }

    public IEnumerator Dash()
    {
        GetComponent<AgentMovement>().agent.speed = 30;
        HideShowSkinMesh(false);
        dashParticle.SetActive(true);
        dashing = true;
        SoundManager.PlaySound(SoundManager.Sound.Dash, transform.position);

        yield return new WaitForSeconds(.5f);
        GetComponent<AgentMovement>().agent.speed = 8;
        
        HideShowSkinMesh(true);
        yield return new WaitForSeconds(.05f);
        dashParticle.SetActive(false);
        dashing = false;

    }

    public IEnumerator SpellE()
    {
        anim.SetTrigger("SpellAttack");
        yield return null;
    }
    public void ExploadSkill()
    {
        Instantiate(spellParticle, transform.position, Quaternion.identity);

        enemies = new List<GameObject>();

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (10f > Vector3.Distance(transform.position, enemy.transform.position))
            {
                enemy.GetComponent<EnemyHealth>().ModifyHealth(-100f);
            }
        }
    }


    public IEnumerator SwordPassive()
    {
        shouldBuffStats = true;
        stats.damage = stats.damage + 15f;
        swordGlow.SetActive(true);
        yield return new WaitForSeconds(15f);
        shouldBuffStats = false;
        stats.damage = stats.damage - 15f;
        swordGlow.SetActive(false);
    }

    void HideShowSkinMesh(bool state)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer meshes in skinMeshList)
        {
            meshes.enabled = state;
        }
    }

    public void TurnOnCanvas()
    {
        playerUICanvas.SetActive(true);
    }
    public void TurnOffCanvas()
    {
        playerUICanvas.SetActive(false);
    }
}
