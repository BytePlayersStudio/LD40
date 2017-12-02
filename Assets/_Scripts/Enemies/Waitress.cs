/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waitress : MonoBehaviour {

	#region Variables
	// We need a Shooting Range for the Enemies,
	// We also need a vision range for the enemies and shoot a ray to check if they can see the player
	public float shootingRange;
	public float visionRange;
	Vector2 direction;

	//Nodes for the patrol behavior
	public GameObject node1;
	public GameObject node2;

	//Movement variables
	public float moveSpeed;

	//Detection variables
	public bool isInRange;
	public bool canShoot;

	#endregion

	#region Unity Methods

	void Start () 
	{
		if (shootingRange == 0) shootingRange = 5.0f;
		if (visionRange == 0) visionRange= 10.0f;
		if (node1 == null) node1 = this.transform.parent.GetChild(1).gameObject;
		if (node2 == null) node2 = this.transform.parent.GetChild(2).gameObject;
		if (moveSpeed == 0) moveSpeed = 10.0f;
		direction = new Vector2(transform.localScale.x, 0);
	}

	void Update ()
	{
		if (isInRange)
		{
			// We can get closer to the player within the limits of the patrol node.
			// We check if we can shoot.
			if (canShoot) {
				// The Enemy can shoot.
			}
		}
	}

	void FixedUpdate()
	{
		isInRange = InRange(transform,direction, visionRange,"Player", Color.green);
		canShoot = InRange(transform, direction, shootingRange, "Player", Color.red);
	}
	#endregion

	#region Custom Functions

	private void Patrol() {

	}

	private bool InRange(Transform pos, Vector2 direction, float range, string objetiveTag, Color debugColor) {
		
		RaycastHit2D hit = Physics2D.Raycast(pos.position, direction, range);
		Debug.DrawRay(pos.position, direction * range, debugColor);

		if (hit.collider != null && hit.collider.tag == objetiveTag)
		{
			Debug.Log("Player in Range");
			return true;
		}
		else
		{
			Debug.Log("Player is not in Range");
			return false;
		}
	}


	#endregion
}
