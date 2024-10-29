using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3, score = 0;

    [SerializeField] TextMeshProUGUI scoreText, livesText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void AddToLives()
    {
        playerLives++;
        livesText.text = playerLives.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives >1 )
        {
            TakeLife();
        }
        else
        {
            RestGame();
        }
    }

    private void RestGame()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
    }
}
