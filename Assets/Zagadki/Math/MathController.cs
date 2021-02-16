using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MathController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject firstCanvas;
    public GameObject secondCanvas;

    [Header("References and variables")]
    public PlayerController player;
    public bool isOpenFirstCanvas = false;
    public bool isOpenSecondCanvas = false;
    //[SerializeField] private Animator anim;
    public GameObject gate;
    public TMP_InputField finalAnswerInput1;
    public TMP_InputField finalAnswerInput2;

    [Header("Attemp 1")]
    public int counter1;
    public GameObject firstAttempt;
    public GameObject secondAttempt;
    public GameObject thirdAttempt;

    [Header("Attemp 2")]
    public int counter2;
    public GameObject firstAttempt2;
    public GameObject secondAttempt2;
    public GameObject thirdAttempt2;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenFirstCanvas)
        {
            player.GetComponent<AgentMovement>().StopMoving();
        }

        if (isOpenSecondCanvas)
        {
            player.GetComponent<AgentMovement>().StopMoving();
        }
    }

    public void TurnOnCanvas()
    {
        isOpenFirstCanvas = true;
        firstCanvas.SetActive(true);
    }
    public void TurnOffCanvas()
    {
        isOpenFirstCanvas = false;
        firstCanvas.SetActive(false);
    }
    public void TurnOffSecondCanvas()
    {
        secondCanvas.SetActive(false);
    }
    public void Check()
    {
        if (isOpenFirstCanvas == true)
        {
            if (finalAnswerInput1.text == "18")
            {
                TurnOffCanvas();
                gate.GetComponent<Animator>().SetBool("isOpen", true);
                SoundManager.PlaySound(SoundManager.Sound.DoorOpen, transform.position);
            }
            else
            {
                counter1++;
                if (counter1 == 1)
                {
                    firstAttempt.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                }
                else if (counter1 == 2)
                {
                    secondAttempt.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                }
                else if (counter1 == 3)
                {
                    thirdAttempt.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                    StartCoroutine(WaitAndChangeCanvas());
                    counter2 = 0;

                    firstAttempt2.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                    secondAttempt2.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                    thirdAttempt2.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                }
                gate.GetComponent<Animator>().SetBool("isOpen", false);
            }
        }
        else if (isOpenSecondCanvas)
        {
            if (finalAnswerInput2.text == "20")
            {
                secondCanvas.SetActive(false);
                isOpenSecondCanvas = false;
                gate.GetComponent<Animator>().SetBool("isOpen", true);
                player.TurnOnCanvas();
                SoundManager.PlaySound(SoundManager.Sound.DoorOpen, transform.position);
            }
            else
            {
                counter2++;
                if (counter2 == 1)
                {
                    firstAttempt2.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                }
                else if (counter2 == 2)
                {
                    secondAttempt2.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                }
                else if (counter2 == 3)
                {
                    thirdAttempt2.GetComponent<Image>().color = new Color(255, 0, 0, 255);
                    StartCoroutine(WaitAndChangeCanvasToFirst());
                    counter1 = 0;
                    firstAttempt.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                    secondAttempt.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                    thirdAttempt.GetComponent<Image>().color = new Color(35, 225, 0, 255);
                }
                gate.GetComponent<Animator>().SetBool("isOpen", false);
            }
        }
        
        
    }
    private IEnumerator WaitAndChangeCanvas()
    {
        yield return new WaitForSeconds(1f);
        isOpenSecondCanvas = true;
        TurnOffCanvas();
        secondCanvas.SetActive(true);
    }
    private IEnumerator WaitAndChangeCanvasToFirst()
    {
        yield return new WaitForSeconds(1f);
        secondCanvas.SetActive(false);
        TurnOnCanvas();
    }
}
