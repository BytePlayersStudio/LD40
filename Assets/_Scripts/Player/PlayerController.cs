using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float hpPoints;


	void Start () {
        // If life is higher than 0...
		if (IsAlive())
        {

        }
	}
	
	private bool IsAlive()
    {
        if (hpPoints <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
