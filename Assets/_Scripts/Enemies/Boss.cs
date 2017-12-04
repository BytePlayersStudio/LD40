/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	enum State {
		Patrol,
		AttackPlayer
	};
	State currentState;
	#region Variables
	//Bullet variables
	public GameObject player;
	public Transform[] shooters;
	public GameObject[] bulletPrefabs;

	public float bulletSpeed;
	private float lastShot;
	public float DelayBetweenBullets = 1;
	public Transform bulletParent;
	AudioSource shotSound;

	public Transform[] waypoints;
	private int waypointID = 0;

	//Movement Variables
	public float moveSpeed = 10;
	#endregion

	#region Unity Methods

	void Start () 
	{
		if (DelayBetweenBullets == 0) DelayBetweenBullets = 1.0f;
		if (bulletParent == null) Debug.LogError("Bullet Parent not found. Set a parent for the bullets.");
		lastShot = Time.time;
		if (shooters == null) Debug.LogError("Assign a shooter to spawn the bullets");
		if (player == null) GameObject.FindGameObjectWithTag("Player");

		currentState = State.Patrol;
	}

	void Update ()
	{
		if(currentState == State.Patrol)
			Patrol();
		if (currentState == State.AttackPlayer)
			AttackPlayer();


	}
	#endregion

	#region Custom Functions
	private void AttackPlayer()
	{

	}

	private void Patrol()
	{
		if (Mathf.Abs(Vector2.Distance(waypoints[waypointID].position, transform.position)) < .5f)
		{
			changeWaypoint();
		}
		Vector2 translation = Vector2.MoveTowards(transform.position, waypoints[waypointID].transform.position, moveSpeed * Time.deltaTime);
		transform.position = new Vector2(translation.x, translation.y);
		Shoot();
	}
	private void Shoot()
	{

		GameObject bulletPrf;
		bulletPrf = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
		bulletPrf.transform.localScale = new Vector3(.3f, .3f, .3f);

		if (Time.time - lastShot > DelayBetweenBullets)
		{
			foreach (Transform s in shooters)
			{
				var bullet = (GameObject)Instantiate(bulletPrf, s.transform.position, Quaternion.identity);
				bullet.transform.SetParent(bulletParent);
				bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x, -1)
					* bulletSpeed * 10);

				bullet = (GameObject)Instantiate(bulletPrf, s.transform.position, Quaternion.identity);
				bullet.transform.SetParent(bulletParent);
				bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x, -1)
					* bulletSpeed * 10);

				Destroy(bullet, 2.0f);
			}
			//shotSound.Play();
			lastShot = Time.time;
		}
		
	}

	private void changeWaypoint()
	{
		waypointID = Random.Range(0, waypoints.Length);
		Debug.Log(waypointID);
	}
	#endregion
}
