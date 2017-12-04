using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    #region Custom Functions
    
    // Loads the Level Scene
    public void LoadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level");        
    }

    // Loads the BossLevel Scene
    public void LoadBossLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("BossLevel");
    }

    // Loads the Main Menu Scene
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Loads the Credits Scene
    public void LoadCredits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }

    // Closes the game
    public void Exit()
    {
        Application.Quit();
    }

    #endregion
}
