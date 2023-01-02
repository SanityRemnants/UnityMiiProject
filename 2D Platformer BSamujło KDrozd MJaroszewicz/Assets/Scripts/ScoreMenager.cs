using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class ScoreMenager : MonoBehaviour
{
    public static ScoreMenager instance;

    public GameState currentGameState = GameState.GS_GAME;
    public Image[] keysTab;

    public Text scoreText;
    public string lvnumber = "0";
    public Text currentscore;
    public Text bestscore;
    public Canvas InGameCanvas;
    public Canvas pauseCanvas;
    public Canvas victoryCanvas;
    public Text VictoryText;
    public Text message;
    public Text HP;
    private int keysFound = 0;
    private int score = 0;
    public int life = 3;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
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
        currentGameState = GameState.GS_GAME;
        InGameCanvas.enabled = true;
        pauseCanvas.enabled = false;
        victoryCanvas.enabled = false;
        Time.timeScale = 1;
    }
    public void unpause()
    {

        InGameCanvas.enabled = true;
        pauseCanvas
            .enabled = false;
        Time.timeScale = 1;
    }
    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void restart()
    {
        SceneManager.LoadScene("Level_1");
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (newGameState == GameState.GS_GAME)
            InGame();
        else 
            PauseMenu();
    }
    public void ReachExit()
    {
        if (keysFound == keysTab.Length)
        {
           victory();
            LevelCompleted();
        }
        else
        {
            youNeedKey();
        }
    }
    public void PauseMenu()
    {
        currentGameState = GameState.GS_PAUSEMENU;
        InGameCanvas.enabled = false;
        pauseCanvas.enabled = true;
        Time.timeScale = 0;
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
    public int checkKeys()
    {
        return keysFound;
    }
    public void hpUpdate(int a)
    {
        life = a;
        HP.text = "x" + life.ToString();
    }
    public void subhp()
    {
        life -= 1;
        HP.text = "x" + life.ToString();
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

        VictoryText.text = "GAME OVER";
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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_PAUSEMENU) {
                SetGameState(GameState.GS_GAME);
                
            }
            else
            {
                SetGameState(GameState.GS_PAUSEMENU);
            }
        }
    }
}
