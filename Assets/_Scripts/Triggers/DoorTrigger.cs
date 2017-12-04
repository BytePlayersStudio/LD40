/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	#region Variables
	public GameObject boss;
	#endregion

	#region Unity Methods

	void Start () 
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			transform.parent.GetChild(0).gameObject.SetActive(false);
			transform.parent.GetChild(1).gameObject.SetActive(true);
			if (boss.activeInHierarchy == false)
				boss.SetActive(true);
			Destroy(this.gameObject);

		}
	}
	#endregion

	#region Custom Functions
	#endregion
}
