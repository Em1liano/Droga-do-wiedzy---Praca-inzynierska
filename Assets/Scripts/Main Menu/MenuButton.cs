using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator anim;
    [SerializeField] AnimatorFunctions animFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] private GameObject savedText;

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            // Aktywowanie animacji selected, kiedy index jest na kafelce
            anim.SetBool("selected", true);

            if (Input.GetAxis("Submit") == 1)
            {
                // kiedy gracz wciśnie Enter lub spacje to uruchamianie kolejnej animacji 
                anim.SetBool("pressed", true);
            }
            else if (anim.GetBool("pressed"))
            {
                anim.SetBool("pressed", false);
                animFunctions.disableOnce = true;
            }
        }
        else
        {
            anim.SetBool("selected", false);
        }

        // Start Menu
        if (menuButtonController.index == 2 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.StartMenu)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffStartMenuCanvas();
            GetComponentInParent<CanvasManager>().TurnOnOptionsMenuCanvas();
        }
        else if (menuButtonController.index == 3 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.StartMenu)
        {
            Debug.Log("Game quit");
            Application.Quit();
        }
        else if (menuButtonController.index == 1 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.StartMenu)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffStartMenuCanvas();
            GetComponentInParent<CanvasManager>().TurnOnLoadGameCanvas();
        }
        else if (menuButtonController.index == 0 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.StartMenu)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffStartMenuCanvas();
            GetComponentInParent<CanvasManager>().TurnOnCreateNewPlayerCanvas();
        }

        // Options Menu
        if (menuButtonController.index == 0 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.OptionsMenu)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffOptionsMenuCanvas();
            GetComponentInParent<CanvasManager>().TurnOnStartMenuCanvas();
        }

        // Save options to do
        if (menuButtonController.index == 1 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.OptionsMenu)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            FindObjectOfType<OptionsController>().SaveVolume();
            StartCoroutine(SavedTextTimer());
        }


        // Load Game Menu
        if (menuButtonController.index == 0 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.LoadGame)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffLoadGameCanvas();
            GetComponentInParent<CanvasManager>().TurnOnStartMenuCanvas();
        }


        // Create new player MENU
        if (menuButtonController.index == 1 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.CreateNewPlayer)
        {
            SoundManager.PlaySound(SoundManager.Sound.MenuClick);
            GetComponentInParent<CanvasManager>().TurnOffCreateNewPlayerCanvas();
            GetComponentInParent<CanvasManager>().TurnOnStartMenuCanvas();
        }
        else if (menuButtonController.index == 0 && Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<MenuButtonController>().canvasType == CanvasType.CreateNewPlayer)
        {
            GetComponentInParent<CanvasManager>().StartNewGame();
        }



    }

    private IEnumerator SavedTextTimer()
    {
        savedText.SetActive(true);
        yield return new WaitForSeconds(2f);
        savedText.SetActive(false);
    }
    }
