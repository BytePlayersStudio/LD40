/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreObjects : MonoBehaviour {

	#region Variables
	public GameObject[] objectsToIgnore;
	#endregion

	#region Unity Methods

	void Start () 
	{
		foreach (GameObject go in objectsToIgnore)
			if(go.GetComponent<Collider>() != null)
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			Destroy(GetComponent<Collider2D>());
			Destroy(GetComponent<Rigidbody2D>());
		}
		
	}
	#endregion

	#region Custom Functions
	#endregion
}
