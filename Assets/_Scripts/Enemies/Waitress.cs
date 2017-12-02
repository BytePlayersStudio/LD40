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
	public Transform node1;
	public Transform node2;

	//Movement variables
	public float moveSpeed;

	//Detection variables
	public bool isInRange;
	public bool canShoot;

	#endregion

	#region Unity Methods

	void Start () 
	{
		if (shootingRange == 0) shootingRange = 4.0f;
		if (visionRange == 0) visionRange= 5.0f;

		if (node1 == null) node1 = this.transform.parent.GetChild(1);
		if (node2 == null) node2 = this.transform.parent.GetChild(2);

		if (moveSpeed == 0) moveSpeed = 10.0f;

		direction = new Vector2(transform.localScale.x, 0);
	}

	void Update ()
	{
		Patrol();
		if (isInRange)
		{
			// We can get closer to the player within the limits of the patrol node.
			// We check if we can shoot.
			ApproachPlayer();
			if (canShoot)
			{
				// The Enemy can shoot.
				Shoot();
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

	private void Patrol()
	{
		transform.Translate(direction * Time.deltaTime * moveSpeed);
		if (Vector2.Distance(transform.position, node1.position) < .1f) {
			FlipSprite();
		}
		if (Vector2.Distance(transform.position, node2.position) < .1f)
		{
			FlipSprite();
		}
	}

	private void ApproachPlayer()
	{

	}
	private void Shoot()
	{

	}

	private bool InRange(Transform pos, Vector2 direction, float range, string objetiveTag, Color debugColor)
	{
		
		RaycastHit2D hit = Physics2D.Raycast(pos.position, direction, range);
		Debug.DrawRay(pos.position, direction * range, debugColor);

		if (hit.collider != null && hit.collider.tag == objetiveTag)
		{
			//Debug.Log("Player in Range");
			return true;
		}
		else
		{
			//Debug.Log("Player is not in Range");
			return false;
		}
	}

	private void FlipSprite() {
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		direction *= -1;
	}

	#endregion
}
