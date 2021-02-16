using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadingSaveController : MonoBehaviour
{
    // Obiekt "płytka" zapisy (przycisk)
    [SerializeField] private GameObject saveTilePrefab;

    // Dziećmi tego obiektu będą płytki zapisów
    public Transform tilesParent;

    // Start is called before the first frame update
    void Start()
    {
        // Ładowanie płytek
        LoadTiles();
    }

    public void LoadTiles()
    {
#if UNITY_STANDALONE // ten kod wykona się tylko dla wersji na komputer

        string path = Application.streamingAssetsPath;
#elif UNITY_ANDROID // ten kod wykona się tylko w wersji na androida

        string path = Application.persistentDataPath;
#endif
        // pobieranie do tablicy stringów plików z ścieżki path
        string[] files = Directory.GetFiles(path);

        // iterowanie po każdym elemencie tablicy files
        foreach (string file in files)
        {
            // stworzenie separatorów, aby ukrócić wybieranie odpowiedniego pliku zapisy
            char[] separators = { '/', '\\', '.' };

            string[] tokens = file.Split(separators);

            // jeżeli plik kończy się na .meta
            if (file.Split(separators)[tokens.Length - 1].Equals("meta"))
                continue;

            string saveName = file.Split(separators)[tokens.Length - 2];

            // Stworznie płytek i przypisanie do obiektu rodzica
            var tile = Instantiate(saveTilePrefab, tilesParent).GetComponent<SaveTile>();

            // Ustawienie danych dla płytek
            tile.SetData(saveName);

        }
    }
}
