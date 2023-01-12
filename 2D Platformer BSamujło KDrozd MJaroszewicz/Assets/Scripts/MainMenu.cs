using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnLevel2ButtonPressed()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void ResetScoreForLv(string lv)
    {
        PlayerPrefs.SetInt("score"+lv, 0);
    }

    public void OnExitToDekstopButtonPressed()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
