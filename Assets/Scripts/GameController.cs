using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public static GameController instance;
    [SerializeField] private GameObject playerPrefab;
    public PlayerController player;
    public GameObject deathCanvas;
    public GameObject questCanvas;
    public GameObject savedTest;

    [Header("List of Enemies")]
    public List<GameObject> enemiesList;

    [Header("Word number")]
    public int currentWorld;
    public string currentPlayerName;

    [Header("UI")]
    public TextMeshProUGUI questName;
    public TextMeshProUGUI descriptionQuest;
    public TextMeshProUGUI rewardsQuest;

    [Header("Sounds")]
    public SoundAudioClip[] SoundAudioClipArray;

    [Header("SoundTracks")]
    public SoundtrackAudioClip[] SoundTrackAudioClipArray;

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

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.Menu);
        }
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.Level1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.Level2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.Level3);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.Level4);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            SoundManager.PlaySoundTrack(SoundManager.SoundTrack.EndScreen);
        }
    }

    public void RecapturePlayerReference()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    public void StartGame(string name)
    {

        // Znajdowanie odwołania do gracza następnie przypisanie
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.GetComponent<PlayerController>();
        }
        else
        {
            // Do poprawki ładowanie zapisu   
            PlayerData data = SaveSystem.LoadGame(name);
            
            // Tworzenie gracza w określonej pozycji
            obj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player = obj.GetComponent<PlayerController>();

            // Przypisywanie zapisanych statystyk dla stworzonego gracza
            player.stats.level = data.level;
            player.stats.Name = data.Name;
            player.stats.experience = data.experience;
            player.stats.damage = data.damage;
            player.GetComponent<PlayerHealth>().maxHealth = data.health;
            player.GetComponent<Mana>().maxMana = data.mana;

            SceneManager.LoadScene(data.currentWorld);

            player.SetPosition(new Vector3(data.posX, data.posY, data.posZ));
            player.quest = data.currentQuest;
            player.stats.currentWorld = data.currentWorld;
        }

        DetectEnemies();
    }

    private void Update()
    {
        // Klawisz F5 odpowiada za zapis gry
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (player != null)
            {
                SaveGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (player != null)
            {
                Debug.Log("Clean quest");
                player.quest.Complete();
                player.quest.description = null;
                player.quest.title = null;
                player.quest.experienceReward = 0;
            }
        }
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.GetComponent<PlayerController>();
        }

        if (questName != null && player != null)
        {
            questName.text = player.quest.title;
            descriptionQuest.text = player.quest.description;
            rewardsQuest.text = player.quest.experienceReward.ToString();
        }


        if (player != null)
        {
            if (player.isAlive == true)
            {
                deathCanvas.SetActive(false);
            }
        }
       

        DestroySounds();


    }
    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            currentWorld = 0;
            player.stats.currentWorld = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentWorld = 1;
            player.stats.currentWorld = 1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            currentWorld = 2;
            player.stats.currentWorld = 2;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            currentWorld = 3;
            player.stats.currentWorld = 3;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            currentWorld = 4;
            player.stats.currentWorld = 4;
        }
        Debug.Log(currentWorld);

        SaveSystem.SaveGame(player);

        StartCoroutine(SaveTextActivator());
    }
    public int GetCurrentWorld()
    {
        return currentWorld;
    }
    public IEnumerator SaveTextActivator()
    {
        savedTest.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        savedTest.SetActive(false);
    }

    public IEnumerator ShowEndCanvas()
    {
        yield return new WaitForSeconds(1f);
        deathCanvas.SetActive(true);
    }
    public IEnumerator TurnOffEndCanvas()
    {
        deathCanvas.SetActive(false);
        yield return null;
    }

    public void RetryClick()
    {
        Destroy(player.gameObject);
        StartCoroutine(TurnOffEndCanvas());
        InstantiateNewPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InstantiateNewPlayer()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");

        // Do poprawki ładowanie zapisu
        PlayerData data = SaveSystem.LoadGame(FindObjectOfType<Stats>().Name);

        obj = Instantiate(playerPrefab, new Vector3(data.posX, data.posY, data.posZ), Quaternion.identity);
        player = obj.GetComponent<PlayerController>();

       
        // Przypisywanie zapisanych statystyk dla stworzonego gracza
        player.stats.level = data.level;
        player.stats.Name = data.Name;
        player.stats.experience = data.experience;
        player.stats.damage = data.damage;
        player.GetComponent<PlayerHealth>().maxHealth = data.health;
        player.GetComponent<Mana>().maxMana = data.mana;
    }

    public void DetectEnemies()
    {
        enemiesList = new List<GameObject>();

        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemiesList.Add(enemy);
        }
    }

    public void DestroySounds()
    {
        List<GameObject> soundsList = new List<GameObject>();

        foreach (var sound in GameObject.FindGameObjectsWithTag("SFX"))
        {
            soundsList.Add(sound);
            Destroy(sound, 3f);
        }
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
    [System.Serializable]
    public class SoundtrackAudioClip
    {
        public SoundManager.SoundTrack soundTrack;
        public AudioClip audioClip;
    }
}


