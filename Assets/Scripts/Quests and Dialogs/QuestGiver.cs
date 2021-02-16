using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum QuestState
{
    Dialog,
    ReadyToTake,
    InProgress,
    Finished,
    Quest,
    End
}
public class QuestGiver : MonoBehaviour
{
    [Header("References")]
    public Quest quest;
    public PlayerController player;
    public GameObject questWindow;
    public GameObject questMark;
    public GameController gc;
    

    [Header("UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI experienceText;

    [Header("Variables")]
    public bool isShowingCanvas = false;
    public bool isTakenQuest = false;
    public bool shouldShowDialog = false;
    public bool endedDialog = false;

    // Lumberjack
    public bool shouldShowLumberDialog = false;
    public bool isLumberMissionComplited = false;
    public GameObject teleport;
    
    public bool isMissionComplited = false;
    public bool endedSecondDialog = false;

    [Header("Zagadka")]
    public GameObject zagadkaCanvas;
    public GameObject whereKeyCanvas;
    public TMP_InputField answerInputField;

    [SerializeField] private QuestState _currentQuestState;
    private void Start()
    {
        gc = FindObjectOfType<GameController>();

        if (quest.isActive)
        {
            // przypisywanie textu
            gc.questName.text = titleText.text;
            gc.descriptionQuest.text = descriptionText.text;
            gc.rewardsQuest.text = experienceText.text;
        }
    }

    private void Update()
    {
        // Kod zmieniający state zadania
        switch (_currentQuestState)
        {
            case QuestState.Dialog:
                {
                    if (gameObject.tag.Equals("Lumberjack"))
                    {
                        if (PlayerPrefs.HasKey("Second Mission Status"))
                        {
                            _currentQuestState = QuestState.ReadyToTake;
                        }
                        else
                        {
                            if (shouldShowLumberDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas1.SetActive(true);
                                player = FindObjectOfType<PlayerController>();
                                player.GetComponent<AgentMovement>().canMove = false;
                            }
                            if (endedDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas1.SetActive(false);
                                _currentQuestState = QuestState.ReadyToTake;
                            }
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("First Mission Status"))
                        {
                            _currentQuestState = QuestState.ReadyToTake;
                        }
                        else
                        {
                            if (shouldShowDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas1.SetActive(true);
                                player = FindObjectOfType<PlayerController>();
                                player.GetComponent<AgentMovement>().canMove = false;

                            }
                            if (endedDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas1.SetActive(false);
                                _currentQuestState = QuestState.ReadyToTake;
                            }
                        }
                    }
                    break;
                }
            case QuestState.ReadyToTake:
                {
                    if (gameObject.tag.Equals("Lumberjack"))
                    {          
                        if (PlayerPrefs.HasKey("Second Mission Status"))
                        {
                            if (PlayerPrefs.GetInt("Second Mission Status") == 1)
                            {
                                _currentQuestState = QuestState.InProgress;
                            }
                            else if (PlayerPrefs.GetInt("Second Mission Status") == 2)
                            {
                                _currentQuestState = QuestState.Finished;
                            }

                        }
                        else
                        {
                            OpenQuestWindow();
                        }
                        
                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("First Mission Status"))
                        {
                            if (PlayerPrefs.GetInt("First Mission Status") == 1)
                            {
                                _currentQuestState = QuestState.InProgress;
                            }
                            else if (PlayerPrefs.GetInt("First Mission Status") == 2)
                            {
                                _currentQuestState = QuestState.Finished;
                            }
                            else if (PlayerPrefs.GetInt("First Mission Status") == 3)
                            {
                                _currentQuestState = QuestState.End;
                            }
                        }
                        else
                        {
                            player = FindObjectOfType<PlayerController>();

                            if (isShowingCanvas == true)
                            {
                                player.GetComponent<AgentMovement>().canMove = false;
                            }
                            if (isShowingCanvas == false)
                            {
                                player.GetComponent<AgentMovement>().canMove = true;
                            }
                            OpenQuestWindow();
                        }
                    }
                    break;
                }
            case QuestState.InProgress:
                {
                    if (gameObject.tag.Equals("Lumberjack"))
                    {
                        if (PlayerPrefs.HasKey("Second Mission Status"))
                        {
                            questMark.SetActive(false);

                            if (isLumberMissionComplited && FindObjectOfType<PlayerController>().missionLevel3Ref)
                            {
                                _currentQuestState = QuestState.Finished;
                            }
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("First Mission Status"))
                        {
                            questMark.SetActive(false);

                            if (isMissionComplited && FindObjectOfType<PlayerController>().missionRef)
                            {
                                _currentQuestState = QuestState.Finished;
                            }
                        }
                    }
                    break;
                }
            case QuestState.Finished:
                {
                    if (gameObject.tag.Equals("Lumberjack"))
                    {
                        if (PlayerPrefs.HasKey("Second Mission Status"))
                        {
                            questMark.SetActive(false);

                            if (shouldShowLumberDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas2.SetActive(true);
                                player = FindObjectOfType<PlayerController>();
                                player.GetComponent<AgentMovement>().canMove = false;
                            }

                            if (endedSecondDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas2.SetActive(false);
                                player.GetComponent<AgentMovement>().canMove = true;
                                teleport.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.HasKey("First Mission Status"))
                        {
                            questMark.SetActive(false);

                            if (shouldShowDialog == true)
                            {
                                FindObjectOfType<Dialog>().dialogCanvas2.SetActive(true);
                                player = FindObjectOfType<PlayerController>();
                                player.GetComponent<AgentMovement>().canMove = false;

                                if (endedSecondDialog == true)
                                {
                                    FindObjectOfType<Dialog>().dialogCanvas2.SetActive(false);
                                    player.GetComponent<AgentMovement>().canMove = true;
                                    _currentQuestState = QuestState.Quest;
                                }
                            }

                        }
                    }
                    break;
                }
            case QuestState.Quest:
                {
                    FindObjectOfType<Door>().TurnOnKey();
                    player = FindObjectOfType<PlayerController>();
                    player.GetComponent<AgentMovement>().canMove = false;
                    zagadkaCanvas.SetActive(true); 
                    break;
                }
            case QuestState.End:
                {
                    FindObjectOfType<Door>().TurnOnKey();
                    player = FindObjectOfType<PlayerController>();
                    player.GetComponent<AgentMovement>().canMove = true;
                    questMark.SetActive(false);
                    break;
                }
        }         
    }

    public QuestState GetState(QuestState state)
    {
        _currentQuestState = state;
        return state;
    }

    public void OpenQuestWindow()
    {
        isShowingCanvas = true;
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        experienceText.text = quest.experienceReward.ToString();
    }
    public void CloseQuestWindow()
    {
        endedDialog = false;
        player.GetComponent<AgentMovement>().canMove = true;
        shouldShowLumberDialog = false;
        isShowingCanvas = false;
        questWindow.SetActive(false);
        _currentQuestState = QuestState.Dialog;
    }
    public void AcceptQuest()
    {
        _currentQuestState = QuestState.InProgress;
        player.GetComponent<AgentMovement>().canMove = true;
        PlayerPrefs.SetInt("First Mission Status", 1);
        CloseQuestWindow();
        gc.questCanvas.SetActive(true);

        // przypisywanie textu
        gc.questName.text = titleText.text;
        gc.descriptionQuest.text = descriptionText.text;
        gc.rewardsQuest.text = experienceText.text;

        questMark.SetActive(false);
        quest.isActive = true;
        player.quest = quest;

        player.TurnOnCanvas();
    }
    public void AcceptLevel3Quest()
    {
        _currentQuestState = QuestState.InProgress;
        player.GetComponent<AgentMovement>().canMove = true;
        PlayerPrefs.SetInt("Second Mission Status", 1);
        CloseQuestWindow();    
        gc.questCanvas.SetActive(true);

        // przypisywanie textu
        gc.questName.text = titleText.text;
        gc.descriptionQuest.text = descriptionText.text;
        gc.rewardsQuest.text = experienceText.text;

        questMark.SetActive(false);
        quest.isActive = true;
        player.quest = quest;

        player.TurnOnCanvas();
    }
    public void CloseCanvas()
    {
        whereKeyCanvas.SetActive(false);
    }
    public void Check()
    {
        if (answerInputField.text == "28")
        {
            PlayerPrefs.SetInt("First Mission Status", 3);
            zagadkaCanvas.SetActive(false);
            whereKeyCanvas.SetActive(true);
            player = FindObjectOfType<PlayerController>();
            player.GetComponent<AgentMovement>().canMove = true;
            _currentQuestState = QuestState.End;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), other.gameObject.GetComponent<CapsuleCollider>());
        }
    }
}
