using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // odwołanie potrzebne do zapisywania w systemie binarnym

public static class SaveSystem
{
    public static void SaveGame(PlayerController player)
    {
#if UNITY_STANDALONE // ten kod wykona się tylko dla wersji na komputer

        string path = Application.streamingAssetsPath + "/" + player.stats.Name + ".sv";
#elif UNITY_ANDROID // ten kod wykona się tylko w wersji na androida

        string path = Application.persistentDataPath + "/" + player.stats.Name + ".sv";
#endif

        // tworzenie nowego obiektu PlayerData
        PlayerData data = new PlayerData(player);
        Debug.Log(data.Name);
        // tworzenie formattera
        BinaryFormatter formatter = new BinaryFormatter();
        // tworzenie pliku binarnego, który korzysta z podanej wyżej ścieżki do zapisu
        FileStream stream = new FileStream(path, FileMode.Create);
        // serializacja
        formatter.Serialize(stream, data);
        // zamknięcie pliku
        stream.Close();
    }

    // funkcja wczytująca zapisany stan gry, pobierający nazwę gracza
    public static PlayerData LoadGame(string name)
    {
#if UNITY_STANDALONE // ten kod wykona się tylko dla wersji na komputer

        string path = Application.streamingAssetsPath + "/" + name + ".sv";
#elif UNITY_ANDROID // ten kod wykona się tylko w wersji na androida

        string path = Application.persistentDataPath + "/" + name + ".sv";
#endif
        // jeżeli plik z podaną sciężka istnieje to wykonuje się poniższy kod
        if (File.Exists(path))
        {
            // tworzenie nowego formatera binarnego
            BinaryFormatter formatter = new BinaryFormatter();

            // otwarcie pliku
            FileStream stream = new FileStream(path, FileMode.Open);

            // odczyt, desarializacja pliku gry
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            // zamknięcie pliku binarnego
            stream.Close();
            // zwrócenie danych
            return data;
        }
        // jeżeli plik nie istnieej to twory się nowy PlayerData i zapisuje nowego gracza i podstawowymi statystykami
        else
        {
            PlayerData data = new PlayerData(name);
            SaveNewPlayer(data);
            return data;
        }
    }

    private static void SaveNewPlayer(PlayerData data)
    {
#if UNITY_STANDALONE // ten kod wykona się tylko dla wersji na komputer

        string path = Application.streamingAssetsPath + "/" + data.Name + ".sv";
#elif UNITY_ANDROID // ten kod wykona się tylko w wersji na androida


        string path = Application.persistentDataPath + "/" + data.Name + ".sv";
#endif

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        
    }
    public static void NewGame(string name)
    {
#if UNITY_STANDALONE
        string path = Application.streamingAssetsPath + "/" + name + ".sv";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath + "/" + name + ".sv";
#endif
        PlayerData data = new PlayerData(name);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
}
