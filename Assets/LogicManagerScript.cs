using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    public int playerScore = 0;
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverScreen;
    public AudioSource points_audio;
    birb_script birb_Script;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("high_score");
        highScoreText.text = "High Score: " + highScore.ToString();
        birb_Script = GameObject.FindGameObjectWithTag("birb").GetComponent<birb_script>();
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

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void gameOver()
    {
        if(playerScore > PlayerPrefs.GetInt("high_score"))
        {
            PlayerPrefs.SetInt("high_score", playerScore);
            highScoreText.text = "High Score: " + playerScore;
        }
        gameOverScreen.SetActive(true);
    }
}
