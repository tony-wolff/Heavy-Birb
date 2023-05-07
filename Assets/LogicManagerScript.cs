using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class LogicManagerScript : MonoBehaviour
{
    private int playerScore = 0;
    private bool increaseDifficulty = false;
    // Level (player points) at which difficulty will be increased;
    public List<int>difficultyLevels;
    public Text scoreText;
    public Text highScoreText;
    public Text highScorelegacy;
    public GameObject gameOverScreen;
    public AudioSource points_audio;
    birb_script birb_Script;
    bool pauseBeforeStart = true;
    bool isOver;

    GameObject msgBar;
    private void Start()
    {
        ResetAll();
        isOver = false;
        int highScore=0;
        if(SceneScript.isLegacy)
        {
            highScore = PlayerPrefs.GetInt("high_score_legacy");
            highScorelegacy.text = "High Score: " + highScore.ToString();
        }
        else
        {
            highScore = PlayerPrefs.GetInt("high_score");
            highScoreText.text = "High Score: " + highScore.ToString();
        }
        birb_Script = GameObject.FindGameObjectWithTag("birb").GetComponent<birb_script>();
        Time.timeScale = 0;
        msgBar =  GameObject.FindGameObjectWithTag("messageBar");
    }

    private void Update()
    {
        if (pauseBeforeStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                pauseBeforeStart = false;
                Time.timeScale = 1;
                msgBar.SetActive(false);
            }
        }
        if(getScore() >= 20 && Input.GetKeyDown(KeyCode.RightArrow)){
            msgBar.SetActive(false);
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
            AdjustDifficulty();
            if(getScore() == 20){
            TMP_Text t = msgBar.transform.GetChild(0).GetComponent<TMP_Text>();
            t.text = "Press right arrow to dash";
            msgBar.SetActive(true);
            }
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
        ResetAll();
    }

    public void ResetAll(){
        GameObject.FindGameObjectWithTag("spawner").GetComponent<PipeSpawnerScript>().Reset();
    }

    public void loadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void gameOver()
    {
        isOver = true;
        if(SceneScript.isLegacy)
        {
            if(playerScore > PlayerPrefs.GetInt("high_score_legacy")){
                PlayerPrefs.SetInt("high_score_legacy", playerScore);
                highScorelegacy.text = "High Score: " + playerScore;
            }
        }
        else
        {
            if(playerScore > PlayerPrefs.GetInt("high_score"))
            {
                PlayerPrefs.SetInt("high_score", playerScore);
                highScoreText.text = "High Score: " + playerScore;
            }
        }
        gameOverScreen.SetActive(true);
    }

    public bool IsGameOver()
    {
        return isOver;
    }


    void AdjustDifficulty()
    {
        //BUG HERE
        for(int i=difficultyLevels.Count - 1; i>=0; i--)
        {
            if(difficultyLevels[i] == getScore())
            {
                difficultyLevels.RemoveAt(i);
                increaseDifficulty = true;
            }
        }
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
