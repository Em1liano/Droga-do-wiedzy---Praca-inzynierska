using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogLevel2 : MonoBehaviour
{
    [Header("References Dialog 1")]
    public TextMeshProUGUI textDisplay1;
    public GameObject dialogCanvas1;
    public GameObject continueButton1;
    public bool isOpenEntryCanvas = false;
    public PlayerController player;
    public GameObject Barbarian;

    [Header("Text 1")]
    public string[] sentences1;
    private int index1;
    public float typingSpeed;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(Type());
    }
    private void Update()
    {
        if (textDisplay1.text == sentences1[index1])
        {
            continueButton1.SetActive(true);
        }

        if (isOpenEntryCanvas == true)
        {
            player.GetComponent<AgentMovement>().StopMoving();
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
            Barbarian.gameObject.tag = "null";
            player.TurnOnCanvas();
        }
    }

    public void OpenEntryCanvas()
    {
        dialogCanvas1.SetActive(true);
        isOpenEntryCanvas = true;
    }
    public void CloseEntryCanvas()
    {
        dialogCanvas1.SetActive(false);
        isOpenEntryCanvas = false;
    }
}
