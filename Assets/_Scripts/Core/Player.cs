using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Stat stats;

	// Use this for initialization
	private void Awake()
    {
        stats.Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stats.CurrentFat -= 10;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stats.CurrentFat += 10;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            stats.CurrentCrit -= 10;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            stats.CurrentCrit += 10;
        }
    }
}
