using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    private int playerScore = 40;
    private bool increaseDifficulty = false;
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverScreen;
    public AudioSource points_audio;
    birb_script birb_Script;
    bool pauseBeforeStart = true;
    bool isOver;
    private void Start()
    {
        isOver = false;
        int highScore = PlayerPrefs.GetInt("high_score");
        highScoreText.text = "High Score: " + highScore.ToString();
        birb_Script = GameObject.FindGameObjectWithTag("birb").GetComponent<birb_script>();
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (pauseBeforeStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                pauseBeforeStart = false;
                Time.timeScale = 1;
                GameObject.FindGameObjectWithTag("spacebarimage").SetActive(false);
            }
        }
        else
        {
            AdjustDifficulty();
        }

    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        //If game over, score is not updated
        if (!gameOverScreen.activeSelf)
        {
            playerScore += scoreToAdd;
            scoreText.text = playerScore.ToString();
            points_audio.Play();
            if (!SceneScript.isLegacy)
                birb_Script.IncreaseBirbSize();
        }

    }

    public int getScore()
    {
        return playerScore;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.FindGameObjectWithTag("spawner").GetComponent<PipeSpawnerScript>().Reset();

    }

    public void loadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void gameOver()
    {
        isOver = true;
        if(playerScore > PlayerPrefs.GetInt("high_score"))
        {
            PlayerPrefs.SetInt("high_score", playerScore);
            highScoreText.text = "High Score: " + playerScore;
        }
        gameOverScreen.SetActive(true);
    }

    public bool IsGameOver()
    {
        return isOver;
    }


    void AdjustDifficulty()
    {
        if(getScore() == 10 || getScore() == 20 || getScore() == 30 || getScore() == 40)
            increaseDifficulty = true;
    }

    public void setDifficulty(bool d)
    {
        increaseDifficulty = d;
    }

    public bool IncreaseDifficultyOn()
    {
        return increaseDifficulty;
    }
}
