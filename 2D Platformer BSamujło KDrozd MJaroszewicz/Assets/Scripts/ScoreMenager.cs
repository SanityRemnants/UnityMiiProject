using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class ScoreMenager : MonoBehaviour
{
    public static ScoreMenager instance;

    // public GameState currentGameState = GameState.GS_PAUSEMENU;
    public Image[] keysTab;

    public Text scoreText;
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
    }

   /* void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
    }*/
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
    }

    public void death()
    {
        VictoryText.text = "GAME OVER";
    }
    // Update is called once per frame
    void Update()
    {
        if (message.enabled && (Time.time >= timeWhenDisappear))
        {
            message.enabled = false;
        }
    }
}
