using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignController : MonoBehaviour
{
    // Referencje
    [Header("References")]
    public GameObject mainCanvas;
    public PlayerController player;
    private Animator anim;
    public TMP_InputField firstInput;
    public TMP_InputField secondInput;
    public TMP_InputField thrirdInput;

    // Zmienne
    [Header("Variables")]
    public bool isOpen = false;
    private int textNumber;

    public void OpenCanvas()
    {
        isOpen = true;
        mainCanvas.SetActive(true);
    }
    public void CloseCanvas()
    {
        isOpen = false;
        FindObjectOfType<PlayerController>().TurnOnCanvas();
        mainCanvas.SetActive(false);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponentInParent<Animator>();
    }
    
    private void Update()
    {
        if (isOpen == true)
        {
            player.GetComponent<AgentMovement>().StopMoving();
        }
    }
    public void Check()
    {
        if (firstInput.text == "1" && secondInput.text == "2" && thrirdInput.text == "3")
        {
            CloseCanvas();
            Debug.Log("GOOD");
            anim.SetBool("isOpen", true);
        }
        else
        {
            Debug.Log("Bad");
            anim.SetBool("isOpen", false);
        }
    }
}
