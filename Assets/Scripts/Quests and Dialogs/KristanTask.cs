using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KristanTask : MonoBehaviour
{
    public GameObject kristanCanvas;
    private PlayerController player;
    public TMP_InputField answer;
    public GameObject gate;
    public GameObject wykrzyknik;

    public bool isTaskComplited = false;
    public bool isOpenCanvas = false;

    // Update is called once per frame
    void Update()
    {
        if (isOpenCanvas)
        {
            player = FindObjectOfType<PlayerController>();
            player.GetComponent<AgentMovement>().canMove = false;
        }
        else
        {
            player = FindObjectOfType<PlayerController>();
            player.GetComponent<AgentMovement>().canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKristanCanvas();
        }
        if (isTaskComplited)
        {
            wykrzyknik.SetActive(false);
        }
    }

    public void OpenKristanCanvas()
    {
        kristanCanvas.SetActive(true);
        isOpenCanvas = true;
    }
    public void CloseKristanCanvas()
    {
        kristanCanvas.SetActive(false);
        isOpenCanvas = false;
    }
    public void Check()
    {
        if (answer.text == "7")
        {
            gate.GetComponent<Animator>().SetBool("isOpen", true);
            SoundManager.PlaySound(SoundManager.Sound.DoorOpen, transform.position);
            isTaskComplited = true;
            CloseKristanCanvas();
            player.TurnOnCanvas();
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
