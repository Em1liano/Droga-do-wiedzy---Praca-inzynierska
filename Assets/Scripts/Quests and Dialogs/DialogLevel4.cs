using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogLevel4 : MonoBehaviour
{
    [Header("References Dialog 1")]
    public TextMeshProUGUI textDisplay1;
    public GameObject dialogCanvas1;
    public GameObject continueButton1;
    public bool isOpenEntryCanvas = false;
    public PlayerController player;
    public GameObject Troll;
    public bool afterEntryDialog = false;
    public bool killedAllEnemies = false;
    public GameObject questMark;

    [Header("Text 1")]
    public string[] sentences1;
    private int index1;
    public float typingSpeed;

    [Header("References Dialog 2")]
    public TextMeshProUGUI textDisplay2;
    public GameObject dialogCanvas2;
    public GameObject continueButton2;

    [Header("Text 2")]
    public string[] sentences2;
    private int index2;

    [Header("Robaki")]
    public List<GameObject> enemiesCount;

    private void Awake()
    {
       // DetectEnemiesLevel4();
    }

    private void Start()
    {
        InvokeRepeating("CheckEnemies", 5f, 1f);
        StartCoroutine(Type());
        StartCoroutine(Type2());
        
    }
    private void Update()
    {
        if (textDisplay1.text == sentences1[index1])
        {
            continueButton1.SetActive(true);
        }
        if (textDisplay2.text == sentences2[index2])
        {
            continueButton2.SetActive(true);
        }

        

    }
    IEnumerator Type()
    {
        foreach (char letter in sentences1[index1].ToCharArray())
        {
            textDisplay1.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    IEnumerator Type2()
    {
        foreach (char letter in sentences2[index2].ToCharArray())
        {
            textDisplay2.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton1.SetActive(false);

        if (index1 < sentences1.Length - 1)
        {
            index1++;
            textDisplay1.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay1.text = "";
            continueButton1.SetActive(false);
            CloseEntryCanvas();
            afterEntryDialog = true;
            player.TurnOnCanvas();
            questMark.SetActive(false);
        }
    }
    public void NextSentence2()
    {
        continueButton2.SetActive(false);

        if (index2 < sentences2.Length - 1)
        {
            index2++;
            textDisplay2.text = "";
            StartCoroutine(Type2());
        }
        else
        {
            textDisplay2.text = "";
            continueButton2.SetActive(false);
            questMark.SetActive(false);
            CloseEndCanvas();
            player.TurnOnCanvas();
            Troll.gameObject.tag = "null";
            questMark.SetActive(false);
        }
    }

    public void OpenEntryCanvas()
    {
        dialogCanvas1.SetActive(true);
        player = FindObjectOfType<PlayerController>();
        player.GetComponent<AgentMovement>().canMove = false;
        isOpenEntryCanvas = true;
    }
    public void CloseEntryCanvas()
    {
        dialogCanvas1.SetActive(false);
        player = FindObjectOfType<PlayerController>();
        player.GetComponent<AgentMovement>().canMove = true;
        isOpenEntryCanvas = false;
    }
    public void OpenEndCanvas()
    {
        dialogCanvas2.SetActive(true);
        player = FindObjectOfType<PlayerController>();
        player.GetComponent<AgentMovement>().canMove = false;
    }
    public void CloseEndCanvas()
    {
        dialogCanvas2.SetActive(false);
        player = FindObjectOfType<PlayerController>();
        player.GetComponent<AgentMovement>().canMove = true;
    }

    public void DetectEnemiesLevel4()
    {
        enemiesCount = new List<GameObject>();

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.gameObject.GetComponent<EnemyHealth>().currentHealth > 0)
            {
                enemiesCount.Add(enemy);
            }
            else
            {
                enemiesCount.Remove(enemy);
            }
        }
    }

    private void CheckEnemies()
    {
        DetectEnemiesLevel4();

        if (enemiesCount.Count == 6)
        {
            Debug.Log("checking");
            questMark.SetActive(true);
            killedAllEnemies = true;
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
