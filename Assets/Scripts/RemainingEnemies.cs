using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainingEnemies : MonoBehaviour
{
    public GameSession gameSession;
    public GameObject LevelTransition;

    private void Start()
    {
        //LevelTransition = GameObject.Find("LevelTransition");
        gameSession = FindObjectOfType<GameSession>();
        gameSession.UpdateEnemiesRemaining();
        gameSession.UpdateSceneName();
        //adding this here until i make another script for testing
    }

    private void Update()
    {
        if (GetEnemyCount() <= 0)
        {
            Debug.Log("should be hit");
            LevelTransition.SetActive(true);
        }
    }
    public int GetEnemyCount()
    {
        return transform.childCount;
    }
}
