using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("References Dialog 1")]
    public TextMeshProUGUI textDisplay1;
    public GameObject dialogCanvas1;
    public GameObject continueButton1;

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


    private void Start()
    {
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
        foreach(char letter in sentences1[index1].ToCharArray())
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
            FindObjectOfType<QuestGiver>().endedDialog = true;
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
            FindObjectOfType<QuestGiver>().endedSecondDialog = true;
        }
    }
}
