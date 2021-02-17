using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardLevel4Dialog : MonoBehaviour
{
    [Header("References Dialog 1")]
    public TextMeshProUGUI textDisplay1;
    public GameObject dialogCanvas1;
    public GameObject continueButton1;
    public bool isOpenEntryCanvas = false;
    public PlayerController player;
    public GameObject Fox;
    private Animator anim;

    [Header("Text 1")]
    public string[] sentences1;
    private int index1;
    public float typingSpeed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Type());
    }
    private void Update()
    {
        if (textDisplay1.text == sentences1[index1])
        {
            continueButton1.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextSentence();
        }

        float distance = Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
        
        if (distance > 15f)
        {
            anim.SetTrigger("StandUp");
        }
        else
        {
            anim.SetTrigger("SitDown");
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
            Fox.gameObject.tag = "null";
            player.TurnOnCanvas();
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
}
