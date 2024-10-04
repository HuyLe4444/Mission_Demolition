using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mode : MonoBehaviour
{
    public void playEasyMode()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void playNormalMode()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void playHardMode()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
