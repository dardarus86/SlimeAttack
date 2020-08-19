using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerlives = 5;
    [SerializeField] int playerScore = 0;
    [SerializeField] int level = 1;
    [SerializeField] TextMeshProUGUI playerLiveText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        playerLiveText.text = playerlives.ToString();
        playerScoreText.text = playerScore.ToString();
        levelText.text = level.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

    public void Removelife()
    {
        playerlives--;
    }


}
