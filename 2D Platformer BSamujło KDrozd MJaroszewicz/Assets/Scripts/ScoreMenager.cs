using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenager : MonoBehaviour
{
    public static ScoreMenager instance;

    public Text scoreText;
    public Text VictoryText;
    public Text message;
    private int score = 0;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreText.text = score.ToString() + " POINTS";
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
