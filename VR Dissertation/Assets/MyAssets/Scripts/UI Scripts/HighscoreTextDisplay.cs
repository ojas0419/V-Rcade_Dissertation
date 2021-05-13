using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighscoreTextDisplay : MonoBehaviour
{
    public int highscoreValue;
    TextMeshProUGUI highscoreText;
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        highscoreText = GetComponent<TextMeshProUGUI>();

        // Get the Key for the level and assign the highscore to it
        if(PlayerPrefs.HasKey(levelName))
        {
            highscoreValue = PlayerPrefs.GetInt(levelName);
        }
    }

    public void Update()
    {
        highscoreText.text = "Highscore: " + highscoreValue;

        // for testing only
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResetHiScore();
        }    
    }

    public void ResetHiScore()
    {
        PlayerPrefs.DeleteAll();
        highscoreText.text = "Highscore: " + "0";
    }
}
