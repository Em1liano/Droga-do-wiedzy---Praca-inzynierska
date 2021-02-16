using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.5f;

    void Start()
    {
        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
    }

    private void Update()
    {
        var gameController = FindObjectOfType<GameController>();
        if (gameController)
        {
            SoundManager.SetVolume(volumeSlider.value);
        }
        else
        {
            Debug.LogWarning("No music player found...");
        }
    }
    public void SaveVolume()
    {
        PlayerPrefsController.SetMasterVolume(volumeSlider.value);
    }
    public void SetDefaults()
    {
        volumeSlider.value = defaultVolume;
    }
}
