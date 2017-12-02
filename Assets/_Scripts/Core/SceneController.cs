using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    #region Custom Functions

    // Loads the Level Scene
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    // Loads the Main Menu Scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Loads the Credits Scene
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    // Closes the game
    public void Exit()
    {
        Application.Quit();
    }

    #endregion
}
