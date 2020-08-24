using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerlives = 5;
    [SerializeField] int playerScore = 0;
    [SerializeField] int level = 1;
    [SerializeField] TextMeshProUGUI playerLiveText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI enemiesLeftText;

    // Start is called before the first frame update

   public RemainingEnemies remainingEnemies;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        remainingEnemies = FindObjectOfType <RemainingEnemies>();
        playerLiveText.text = playerlives.ToString();
        playerScoreText.text = playerScore.ToString();
        levelText.text = SceneManager.GetActiveScene().name.ToString();
        enemiesLeftText.text = remainingEnemies.GetEnemyCount().ToString();



    }

    public void UpdateEnemiesRemaining()
    {
        remainingEnemies = FindObjectOfType<RemainingEnemies>();
        enemiesLeftText.text = remainingEnemies.GetEnemyCount().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        enemiesLeftText.text = remainingEnemies.GetEnemyCount().ToString();

    }

    public void AddScore(int score)
    {
        playerScore += score;
        playerScoreText.text = playerScore.ToString();
    }

    public void Removelife()
    {
        playerlives--;
        playerLiveText.text = playerlives.ToString();
    }

    public int GetPlayerLife()
    {
        return playerlives;
    }

    public void UpdateSceneName()
    {
        levelText.text = SceneManager.GetActiveScene().name.ToString();
    }






}
