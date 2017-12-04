using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    #region Variables

    private bool _paused;

    public GameObject pauseMenu;
    public GameObject statsBar;

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start () {

		_paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		//if (SceneManager.GetActiveScene().name != "MainMenu")
		//{
            if (Input.GetButtonDown("Pause") && !_paused)
			{
                Debug.Log("Pausa");
                Time.timeScale = 0f;
				ShowPauseMenu();
			}
		//	}
			else if (Input.GetButtonDown("Pause") && _paused)
			{
                Debug.Log("DesPausa");
                Time.timeScale = 1;
				HidePauseMenu();
			}
		
	}

    #endregion

    #region Custom Regions

    public void ShowPauseMenu()
    {
		
        _paused = true;
	//	if(pauseMenu != null)
		pauseMenu.SetActive(true);
		//if (statsBar != null)
		statsBar.SetActive(false);
    }

    public void HidePauseMenu()
    {
        _paused = false;
	//	if (pauseMenu != null)
		pauseMenu.SetActive(false);
	//	if (statsBar != null)
		statsBar.SetActive(true);
    }



    #endregion
}
