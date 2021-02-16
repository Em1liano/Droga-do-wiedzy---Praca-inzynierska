using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    PlayerController p;
    private void Awake()
    {
        // wzorzec projektowy Singleton
        if (instance != null)
        {
            // jeżeli istnieje już instancja GameControllera to zniszcz
            Destroy(gameObject);
        }
        // przypisanie instancji
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Funckja przyjmuję stringa trzymający nazwę gracza
    public void LoadGame(string name)
    {
        
        // rozpoczącie coroutine
        StartCoroutine(LoadGameAsync(name));
        // Naprawienie błędu wyświetlania menu pauzy podczas rozpoczęciu rozgrywki
        FindObjectOfType<Pause>().ResumeGame();
    }

    public IEnumerator LoadGameAsync(string name)
    {
        while (SceneManager.GetActiveScene().buildIndex != 0)
        {
            yield return null;
        }
        // Wczytanie odpowiedniego zapisu gry za pomocą instancji GameControllera
        GameController.instance.StartGame(name);
    }
    // Funckja stwarzająca nowego gracza
    public void NewGame(string name)
    {
        // pobiera wpisaną nazwę gracza i przypisuje do zapisu
        SaveSystem.NewGame(name);
        StartCoroutine(LoadGameAsync(name));
    }
}
