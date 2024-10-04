using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S; //a private Singleton

    [Header("Inscribed")]
    public TextMeshProUGUI  uitLevel; //The UIText_Level Text
    public TextMeshProUGUI  uitShots; //The UIText_Shots Text
    public TextMeshProUGUI  uitButton; //The Text on UIButton_View
    public Vector3 castlePos; //The place to put castles
    public GameObject[] castles; //An array of the castles

    [Header("Dynamic")]
    public int level; //The current level
    public int levelMax; //The number of levels
    public int shotsTaken;
    public GameObject castle; //The current castle
    public GameMode mode = GameMode.idle;
    public Difficulty currentDifficulty;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    void Start()
    {
        S = this; //Define the Singleton

        // Get the difficulty setting
        int difficultyValue = PlayerPrefs.GetInt("Difficulty", 1); // Default to Normal
        currentDifficulty = (Difficulty)difficultyValue;

        // Set shots based on difficulty
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                shotsTaken = 5;
                break;
            case Difficulty.Normal:
                shotsTaken = 3;
                break;
            case Difficulty.Hard:
                shotsTaken = 1;
                break;
        }

        Goal.goalMet = false;
    }

    void UpdateGUI()
    {
        //Show the data in the GUITexts
        uitShots.text = "Shots Left: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        if (shotsTaken < 0) {
            int totalScenes = SceneManager.sceneCountInBuildSettings;
        
        // Load the last scene (GameOver scene)
            SceneManager.LoadScene(totalScenes - 1);
        }

        //Check for level end
        if (Goal.goalMet)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //Static method that allows code anywhere to increment shotsTaken
    public static void SHOT_FIRED()
    {
        S.shotsTaken--;
    }
}