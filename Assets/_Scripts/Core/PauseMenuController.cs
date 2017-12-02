using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    #region Variables

    private bool _paused;
    private float _realTimeScale;

    public GameObject pauseMenu;

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start () {
        _paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Pause") && !_paused)
        {
            _realTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            ShowPauseMenu();
        }
        else if (Input.GetButtonDown("Pause") && _paused)
        {
            _realTimeScale = Time.timeScale;
            Time.timeScale = _realTimeScale;
            HidePauseMenu();
        }
	}

    #endregion

    #region Custom Regions

    public void ShowPauseMenu()
    {
        _paused = true;
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _paused = false;
        pauseMenu.SetActive(false);
    }

    #endregion
}
