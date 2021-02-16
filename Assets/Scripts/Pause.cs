using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseCanvas;
    public static bool isGamePaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void LoadMenu()
    {
        Destroy(FindObjectOfType<PlayerController>().gameObject);
        Destroy(FindObjectOfType<GameController>().gameObject);
        pauseCanvas.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        pauseCanvas.SetActive(false);
        Application.Quit();
    }
}
