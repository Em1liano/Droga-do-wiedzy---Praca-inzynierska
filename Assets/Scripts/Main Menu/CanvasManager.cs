using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

// Stany jakie mogą występować w Menu głównym, służa do poprawnego oznaczania aktualnych elementów UI
public enum CanvasType
{
    StartMenu,
    OptionsMenu,
    LoadGame,
    CreateNewPlayer
}


public class CanvasManager : MonoBehaviour
{
    public Canvas startMenuCanvas;
    public Canvas optionsMenuCanvas;
    public Canvas loadGameCanvas;
    public Canvas createNewPlayerCanvas;
    public GameObject errorImage;
    // Input, który gracz wpisuje tworząc nowego gracza
    public TMP_InputField newPlayer;

    [SerializeField] private bool isShowingErorr;

    // Funkcje włączające i wyłączające obiekty UI
    public void TurnOnStartMenuCanvas()
    {
        startMenuCanvas.gameObject.SetActive(true);
    }
    public void TurnOffStartMenuCanvas()
    {
        startMenuCanvas.gameObject.SetActive(false);
    }
    public void TurnOnOptionsMenuCanvas()
    {
        optionsMenuCanvas.gameObject.SetActive(true);
    }
    public void TurnOffOptionsMenuCanvas()
    {
        optionsMenuCanvas.gameObject.SetActive(false);
    }
    public void TurnOnLoadGameCanvas()
    {
        loadGameCanvas.gameObject.SetActive(true);
    }
    public void TurnOffLoadGameCanvas()
    {
        loadGameCanvas.gameObject.SetActive(false);
    }
    public void TurnOnCreateNewPlayerCanvas()
    {
        createNewPlayerCanvas.gameObject.SetActive(true);
    }
    public void TurnOffCreateNewPlayerCanvas()
    {
        createNewPlayerCanvas.gameObject.SetActive(false);
    }

    // Funkcja włączająca ekran pełnoekranowy
    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        Debug.Log("Fullscreen is " + isFullScreen);
    }
    // Tworzenie nowej gry
    public void StartNewGame()
    {
        // Warunek do poprawienia 
        // jeżeli wpisano więcej niż 3 znaki to przejście dalej
        // jeżeli nie to wyświetlić odpowiedni error

        Regex r = new Regex("^[a-zA-Z0-9]*$");

        string regexText = newPlayer.text;

        if (Regex.IsMatch(regexText,("^[a-zA-Z0-9]*$"), RegexOptions.IgnoreCase) && regexText.Length > 3)
        {
            SaveManager.instance.NewGame(newPlayer.text);
        }
        else
        {
            ErorImage();
        }
    }
    public void ErorImage()
    {
        isShowingErorr = true;
        errorImage.SetActive(true);
    }
    public void HideErrorImage()
    {
        isShowingErorr = false;
        errorImage.SetActive(false);
    }
    private void Update()
    {
        if (isShowingErorr == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideErrorImage();
            }
        }
    }
}
