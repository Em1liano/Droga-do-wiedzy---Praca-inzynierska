using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveTile : MonoBehaviour
{
    // string trzymający nazwę zapisu
    string SaveName;
    // Text który wyświetla nazwę zapisu w UI
    public TextMeshProUGUI NameText;

    // Funkcja przypisująca dane o zapisie
    public void SetData(string name)
    {
        SaveName = name;
        NameText.text = name;
    }

    // Funkcja wczytuje grę
    public void LoadPlayer()
    {
        // Odwołuje się do instancji SaveManagera w którym jest funkcja LoadGame przyjmująca parametr SaveName ustalony wcześniej
        SaveManager.instance.LoadGame(SaveName);
    }
}
