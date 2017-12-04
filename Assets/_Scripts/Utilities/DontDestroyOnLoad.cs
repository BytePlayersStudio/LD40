/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	#region Variables

	#endregion

	#region Unity Methods
	private static DontDestroyOnLoad _instance = null;
	public static DontDestroyOnLoad Instance{ get{ return _instance; }}

	private void Awake()
	{
		if (_instance != null && _instance != this)
			Destroy(this.gameObject);
		else
		{
			_instance = this;
			DontDestroyOnLoad(_instance.gameObject);
		}


	}
	private void Start()
	{


		if (SceneManager.GetActiveScene().name == "Credits")
			Destroy(this.gameObject);
		if (SceneManager.GetActiveScene().name == "MainMenu")
			Destroy(this.gameObject);


	}
	#endregion

	#region Custom Functions
	#endregion
}
