using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemies.Crab
{
    // Zdefiniowane maszyny stanów
    public enum CrabState
    {
        Sleep,
        Introduction,
        Focusing,
        Stage1,
        Stage2,
        AttackMelee,
        Stage2Attack,
        Heal,
        Death,
        Win
    }

    public class CrabController : MonoBehaviour
    {
        // Scriptable object
        public EnemyType enemyType;
        // Referencje
        [Header("References")]
        public SleepZone sleepZone;
        Animator anim;
        public GameObject crabHealthCanvas;
        public PlayerController target;
        public AgentMovement movement;

        [Header("Stats")]
        // Zmienne
        public float timer;
        public float timer2;
        public float healthPerc;
        public float rotationSpeed;
        public bool isAlive = true;
        public float stopDistance;
        public bool isMoving = false;

        // Zmienna startowa pozycja
        private Vector3 startingPos;
        // Zmienna do losowania animacji ataku wręcz
        [SerializeField] private int randomInt;

        // Aktualny stan bossa
        [SerializeField] private CrabState _currentState;

        
        private void Start()
        {
            GetComponent<AgentMovement>().agent.speed = enemyType.speed;
            anim = GetComponent<Animator>();
            startingPos = transform.position;
        }

        private void Update()
        {
            // Kalkulowanie procentów życia przeciwnika, aby można było przejść do innych stanów
            CalculatePercOfHealth();

            
            // Sprawdzanie czy gracz żyje
            if (target != null)
            {
                var obj = FindObjectOfType<PlayerController>();
                if (obj.isAlive == false)
                {
                    // aby następnie przestał atakować
                    _currentState = CrabState.Win;
                }
            }
            switch (_currentState)
            {
                // Pierwszy stan początkowy odpowiedznialny za włączenie canvasu z życiem oraz przejście do kolejnego stanu
                case CrabState.Sleep:
                    {
                        if (sleepZone.IsPlayerInRange())
                        {
                            crabHealthCanvas.SetActive(true);
                            _currentState = CrabState.Introduction;
                        }
                        break;
                    }
                    // Ten stan uruchamia dwie animacje prezentujące przeciwnika po upływie określonego czasu
                case CrabState.Introduction:
                    {
                        anim.SetTrigger("Introduction1");
                        // po upływie animacji początkowej nr 1 przejście do drugiej animacji
                        timer -= Time.deltaTime;

                        if (timer <= 0)
                        {
                            // timer jest potrzebny, aby animacje uruchamiały się odpowiednio po sobie
                            anim.SetTrigger("Introduction2");
                            _currentState = CrabState.Focusing;
                        }
                        break;
                    }
                    // Ten stan pozyskuje komponenty od gracza, ustawia animacje idlei wyłącza kolizje między graczem a potworem
                case CrabState.Focusing:
                    {
                        // bezpieczne zresetowania triggera, aby uniknąć błędów
                        anim.ResetTrigger("Introduction1");
                        anim.SetTrigger("Fight_Idle_1");

                        // Wyszukanie oraz ustawienie celu na graczu
                        SetTarget(FindObjectOfType<PlayerController>());
                        // Włączenie ignoracji kolizji między bossem a graczem, aby gracz nie mógł przesuwać w świecie gry bossa
                        Physics.IgnoreCollision(GetComponent<BoxCollider>(), target.GetComponent<CapsuleCollider>());

                        if (GetComponent<BoxCollider>() != null)
                        {
                            // Obracanie za graczem
                            FaceToTarget();
                            
                            // Kolejny czasomierz, aby po upływie czasu przejść do pierwszego stanu walki
                            timer2 -= Time.deltaTime;

                            if (timer2 <= 0)
                            {
                                _currentState = CrabState.Stage1;
                            }
                        }
                        
                        break;
                    }
                case CrabState.Stage1:
                    {
                        if (isAlive == true)
                        {
                            FaceToTarget();
                            anim.ResetTrigger("Attack_2");
                            anim.ResetTrigger("Attack_3");
                            anim.ResetTrigger("Attack_5");

                            float distance = Vector3.Distance(transform.position, target.transform.position);

                            if (distance >= stopDistance)
                            {
                                isMoving = true;
                                anim.SetBool("isMoving", isMoving);
                                MoveToTarget();
                            }
                            else
                            {
                                movement.StopMoving();
                                isMoving = false;
                                anim.SetBool("isMoving", isMoving);
                                _currentState = CrabState.AttackMelee;
                            }


                            if (healthPerc <= 50)
                            {
                                _currentState = CrabState.Heal;
                            }
                        }

                        break;
                    }
                case CrabState.AttackMelee:
                    {
                        if (isAlive == true)
                        {
                            GetRandomInt();

                            if (GetRandomInt() == 1)
                            {
                                anim.SetTrigger("Attack_2");

                                Debug.Log("Attack1");
                                _currentState = CrabState.Stage1;
                            }
                            else if (GetRandomInt() == 2)
                            {
                                anim.SetTrigger("Attack_3");

                                Debug.Log("Attack2");
                                _currentState = CrabState.Stage1;
                            }
                            else if (GetRandomInt() == 3)
                            {
                                anim.SetTrigger("Attack_5");

                                Debug.Log("Attack3");
                                _currentState = CrabState.Stage1;
                            }


                            if (healthPerc <= 50)
                            {
                                _currentState = CrabState.Heal;
                            }
                        }

                        break;
                    }
                case CrabState.Stage2Attack:
                    {
                        if (isAlive == true)
                        {
                            GetRandomIntStage2();

                            if (GetRandomIntStage2() == 1)
                            {
                                anim.SetTrigger("Attack_4");

                                _currentState = CrabState.Stage2;
                            }
                            else if (GetRandomIntStage2() == 2)
                            {
                                anim.SetTrigger("Attack_1");

                                _currentState = CrabState.Stage2;
                            }
                        }
                        break;
                    }
                case CrabState.Stage2:
                    {
                        anim.SetBool("isStage2", true);
                        if (isAlive == true)
                        {
                            FaceToTarget();
                            anim.ResetTrigger("Attack_4");
                            anim.ResetTrigger("Attack_1");

                            float distance = Vector3.Distance(transform.position, target.transform.position);

                            if (distance >= stopDistance)
                            {
                                isMoving = true;
                                anim.SetBool("isMoving", isMoving);
                                MoveToTarget();
                            }
                            else
                            {
                                movement.StopMoving();
                                isMoving = false;
                                anim.SetBool("isMoving", isMoving);
                                _currentState = CrabState.Stage2Attack;
                            }
                        }
                        break;
                    }
                case CrabState.Heal:
                    {
                        if (isAlive == true)
                        {
                            GetComponent<AgentMovement>().agent.speed = 5;
                            movement.MoveToPoint(startingPos);

                            if (transform.position.x == startingPos.x)
                            {
                                GetComponent<EnemyHealth>().ModifyHealth(+250);
                                _currentState = CrabState.Stage2;
                            }
                        }
                        break;
                    }
                case CrabState.Death:
                    {
                        break;
                    }
                case CrabState.Win:
                    {
                        if (isAlive == true)
                        {
                            GetComponent<AgentMovement>().agent.speed = 5;
                            movement.MoveToPoint(startingPos);
                        }
                        break;
                    }
            }
        }
        private int GetRandomInt()
        {
            randomInt = Random.Range(0, 3) + 1;
            //Debug.Log(randomInt);
            return randomInt;
        }
        private int GetRandomIntStage2()
        {
            randomInt = Random.Range(0, 2) + 1;
            return randomInt;
        }
        private void CalculatePercOfHealth()
        {
            healthPerc = ((GetComponent<EnemyHealth>().currentHealth / enemyType.maxHealth) * 100);
        }
        // Funkcja oblicza rotację jaką musi wykonać, aby być równo obrócona w stronę gracza
        private void FaceToTarget()
        {
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
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

        public bool GetHit(float value)
        {
            GetComponent<EnemyHealth>().ModifyHealth(-value);
            SoundManager.PlaySound(SoundManager.Sound.CrabGetHit, transform.position);
            // Jeżeli gracz ma mniej lub równo 0 życia to wykonuje się funkcja Die, również wyłącza się Canvas pokazujący życie
            if (GetComponent<EnemyHealth>().currentHealth <= 0)
            {
                _currentState = CrabState.Death;
                Die();
                StartCoroutine(GetComponent<EnemyHealth>().TurnOffCanvas());
                return true;
            }
            return false;
        }

        // Funkcja wyłączająca funkcjonalność przeciwnika
        public void Die()
        {
            SoundManager.PlaySound(SoundManager.Sound.CrabDead, transform.position);
            isAlive = false;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            anim.SetBool("isAlive", isAlive);

            //exp
            FindObjectOfType<Stats>().AddExperience(enemyType.expPerKill);

            if (target.quest.isActive)
            {
                target.quest.goal.EnemyKilled();
                if (target.quest.goal.IsReached())
                {
                    FindObjectOfType<Stats>().AddExperience(target.quest.experienceReward);
                    target.quest.Complete();
                    PlayerPrefs.SetInt("Second Mission Status", 2);
                    FindObjectOfType<QuestGiver>().isLumberMissionComplited = true;
                }
            }
        }

        public void MoveToTarget()
        {
            if (target != null)
            {
                movement.MoveToPoint(target.transform.position);
            }
        }

        public void StartupSFX()
        {
            SoundManager.PlaySound(SoundManager.Sound.CrabStartup, transform.position);
        }
        public void Attack1SFX()
        {
            SoundManager.PlaySound(SoundManager.Sound.CrabAttack1, transform.position);
        }
        public void Attack2SFX()
        {
            SoundManager.PlaySound(SoundManager.Sound.CrabAttack2, transform.position);
        }

        public void HitMelee()
        {
            if (target != null)
            {
                target.GetHit(enemyType.damage);  
            }
        }
    }
}