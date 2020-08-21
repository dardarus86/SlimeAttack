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

    // Start is called before the first frame update
    Player player;

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
        playerLiveText.text = playerlives.ToString();
        playerScoreText.text = playerScore.ToString();
        levelText.text = level.ToString();
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }

        if(player.GetComponent<Rigidbody2D>().IsTouchingLayers(LayerMask.GetMask("LevelTransition")))
        {
            FindObjectOfType<LevelLoader>().LoadNextLevel();
        }
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




}
