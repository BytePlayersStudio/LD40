/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_loader : MonoBehaviour {

	#region Variables
	public string sceneName;
	#endregion

	#region Unity Methods

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Initiate.Fade(sceneName, Color.black, 2);
		}
	}
	#endregion

}
