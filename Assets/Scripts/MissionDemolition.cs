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
    public int shotsTaken = 3;
    public GameObject castle; //The current castle
    public GameMode mode = GameMode.idle;

    void Start()
    {
        S = this; //Define the Singleton

        if (PlayerPrefs.GetInt("HardMode", 0) == 1) {
            shotsTaken = 1;
        } else if (PlayerPrefs.GetInt("NorMode", 0) == 1) {
            shotsTaken = 3;
        } else {
            shotsTaken = 5;
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