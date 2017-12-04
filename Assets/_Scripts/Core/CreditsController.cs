using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

    public SceneController sc;
		
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            returnMenu();
        }
	}

    void returnMenu()
    {
        sc.LoadMainMenu();
    }
}
