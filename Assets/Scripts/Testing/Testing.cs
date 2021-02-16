using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private LevelWindow levelStats;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        //Stats levelSystem = new Stats();
        //levelStats.SetLevelSystem(levelSystem);
        //player.SetLevelSystem(levelSystem);

        //levelSystem.AddExperience(100);
        //Debug.Log(levelSystem.GetLevelNumber());
        //levelSystem.AddExperience(50);
        //Debug.Log(levelSystem.GetLevelNumber());
    }
}
