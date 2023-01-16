using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS, GS_INTRO }

public class ScoreMenager : MonoBehaviour
{
    public static ScoreMenager instance;

    public GameState currentGameState = GameState.GS_GAME;
    public Image[] keysTab;

    public Text victoryText;
    public Text scoreText;
    public string lvnumber = "0";
    public Text currentscore;
    public Text bestscore;
    public Text cooldown;
    public Text currentQuality;
    public Canvas InGameCanvas;
    public Canvas pauseCanvas;
    public Canvas victoryCanvas;
    public Canvas optionsCanvas;
    public Canvas IntroCanvas;
    public Text VictoryText;
    public Text message;
    public Text HP;
    private int keysFound = 0;
    private int score = 0;
    public int life = 3;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    private bool playerAlive = true;
    public Button continueButton;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.gray;
        }
    }
    void Start()
    {

        scoreText.text = score.ToString() + " POINTS";
        HP.text = "x" + life.ToString();

        if (IntroCanvas != null)
        {
            InGameCanvas.enabled = false;
            IntroCanvas.enabled = true;
            currentGameState = GameState.GS_INTRO;
        }
        else
        {
            currentGameState = GameState.GS_GAME;
            InGameCanvas.enabled = true;
        }
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = false;
        victoryCanvas.enabled = false;
        Time.timeScale = 1;
    }
 
    public void SetVolume(float Value)
    {
        AudioListener.volume = Value;
    }
    public bool CheckIfPlayerAlive()
    {
        return playerAlive;
    }
    public void SetPlayerAlive(bool state)
    {
        playerAlive = state;
    }
    public void quality_inc()
    {
        if (QualitySettings.names[QualitySettings.GetQualityLevel()] != QualitySettings.names[QualitySettings.names.Length-1])
            QualitySettings.IncreaseLevel();
        currentQuality.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
        Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }
    public void quality_dec()
    {
        if(QualitySettings.names[QualitySettings.GetQualityLevel()]!= QualitySettings.names[0])
        QualitySettings.DecreaseLevel();
        currentQuality.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
        Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()]);

    }
    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void restart()
    {

        SceneManager.LoadScene("Level_"+lvnumber.ToString());
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (newGameState == GameState.GS_GAME)
            InGame();
        else 
            PauseMenu();
    }
    public bool ReachExit()
    {
        if (keysFound == keysTab.Length)
        {
           victory();
            LevelCompleted();
            return true;
        }
        else
        {
            youNeedKey();
            return false;
        }
    }
    public void loadLvl(string nr)
    {
        SceneManager.LoadScene("Level_"+nr);
    }
    public void ReachRespawn()
    {
        message.enabled = true;
        message.text = "You reach checkpoint!";
        timeWhenDisappear = Time.time + timeToAppear;
    }
    public void options()
    {
        currentQuality.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
        currentGameState = GameState.GS_OPTIONS;
        pauseCanvas.enabled = false;
        optionsCanvas.enabled = true;
        Time.timeScale = 0;
    }
    public void PauseMenu()
    {

        currentGameState = GameState.GS_PAUSEMENU;
        optionsCanvas.enabled = false;
        InGameCanvas.enabled = false;
        pauseCanvas.enabled = true;
        Time.timeScale = 0;
    }
    public void setCooldown(float time)
    {
        if (time > 0.0)
        {
            decimal cd = Math.Round((decimal)time, 3);
            cooldown.text = "Attack cooldown: " + cd.ToString();
        }
        else
        {
            cooldown.text = null;
        }
    }
    public void InGame()
    {
        currentGameState = GameState.GS_GAME;
        InGameCanvas.enabled = true;
        pauseCanvas
            .enabled = false;
        Time.timeScale = 1;
    }
    public void LevelCompleted()
    {
        currentGameState = GameState.GS_LEVELCOMPLETED;
        Time.timeScale = 0;
        InGameCanvas.enabled = false;
        pauseCanvas.enabled = false;
        victoryCanvas.enabled = true;
    }
    public void GameOver()
    {
        currentGameState = GameState.GS_GAME_OVER;
        Time.timeScale = 0;
        InGameCanvas.enabled = false;
        pauseCanvas.enabled = false;
        victoryCanvas.enabled = true;
    }
    public void youNeedKey()
    {
        message.enabled = true;
        message.text = "You need to find key";
        timeWhenDisappear = Time.time + timeToAppear;
    }
    public void addPoint(int a)
    {
        score+= a;
        scoreText.text = score.ToString() + " POINTS";
    }
    public void addhp()
    {
        life += 1;
        HP.text ="x"+ life.ToString();
    }
    public void addkey()
    {
        keysFound += 1;
        keysTab[keysFound - 1].color = Color.white;
    }
    public void subhp()
    {
        life -= 1;
        HP.text = "x" + life.ToString();
        SetPlayerAlive(false);
    }
    public void victory()
    {
        VictoryText.text = "VICTORY";
        if (score > PlayerPrefs.GetInt("score"+lvnumber, 0))
        PlayerPrefs.SetInt("score"+lvnumber, score);

        currentscore.text = "Score: " + score.ToString();
        bestscore.text = "Best Score: " + PlayerPrefs.GetInt("score"+lvnumber, 0).ToString();
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("score"+lvnumber, 0);
        bestscore.text = "Best Score: " + PlayerPrefs.GetInt("score"+lvnumber, 0).ToString();
    }
    public void death()
    {
        if(continueButton!=null)
        continueButton.gameObject.SetActive(false);
        VictoryText.text = "GAME OVER";
        victoryText.fontSize = 20;
        currentscore.text = "Score: " + score.ToString();
        bestscore.text = "Best Score: " + PlayerPrefs.GetInt("score" + lvnumber, 0).ToString();
        GameOver();

    }
    void Update()
    {
        if (message.enabled && (Time.time >= timeWhenDisappear))
        {
            message.enabled = false;
        }
        if ((Input.GetKeyUp(KeyCode.Escape))&&currentGameState!=GameState.GS_INTRO)
        {
            if(currentGameState == GameState.GS_PAUSEMENU) {
                SetGameState(GameState.GS_GAME);
                
            }
            else
            {
                SetGameState(GameState.GS_PAUSEMENU);
            }
        }
        if (currentGameState == GameState.GS_INTRO)
        {
            if (Input.anyKey)
            {
                IntroCanvas.enabled = false;
                SetGameState(GameState.GS_GAME);
            }
        }
    }
}
