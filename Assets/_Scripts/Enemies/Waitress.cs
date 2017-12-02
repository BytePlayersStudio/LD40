/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waitress : MonoBehaviour {

	enum State {
		Patrol,
		Chasing,
		Shooting
	};
	State currentState;

	#region Variables
	//Components
	Rigidbody2D rb;

	// We need a Shooting Range for the Enemies,
	// We also need a vision range for the enemies and shoot a ray to check if they can see the player
	public float shootingRange;
	public float visionRange;
	Vector2 direction;

	//Nodes for the patrol behavior
	public Transform node1;
	public Transform node2;
	[HideInInspector]
	public Transform activeNode;

	//Movement variables
	public float moveSpeed;
	private float moveSpeedIncreased;
	public float speedIncreaseFactor;

	//Related to player variables.

	public GameObject player;
	float distanceFromPlayer; //Absolute distance from player
	Vector2 directionToPlayer;

	//Detection variables

	private bool isInRange;
	private bool canShoot;


	private bool facingRight;
	//Shooting mechanics
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;
	private float lastShot;
	public float DelayBetweenBullets = 1;

	//Animator Variables
	Animator anim_controller;
	bool isAttacking;
	#endregion

	#region Unity Methods

	void Start () 
	{
		if (player == null) Debug.LogError("Player not found.");

		if (shootingRange == 0) shootingRange = 4.0f;
		if (visionRange == 0) visionRange= 5.0f;

		if (node1 == null) node1 = this.transform.parent.GetChild(1);
		if (node2 == null) node2 = this.transform.parent.GetChild(2);

		if (moveSpeed == 0) moveSpeed = 10.0f;

		if(rb == null) rb = transform.GetComponent<Rigidbody2D>();
		if (DelayBetweenBullets == 0) DelayBetweenBullets = 1.0f;
		direction = new Vector2(transform.localScale.x, 0);
		moveSpeedIncreased = moveSpeed * speedIncreaseFactor;
		activeNode = node1;

		lastShot = Time.time;

		facingRight = true;
		currentState = State.Patrol;

		anim_controller = transform.GetComponentInChildren<Animator>();
	}

	void Update ()
	{
		if (currentState == State.Patrol)
		{
			isAttacking = false;
			Patrol();
			setAnimations();
		}
		if (currentState == State.Chasing)
		{
			isAttacking = false;
			ApproachPlayer();
		}
		if (currentState == State.Shooting)
		{
			isAttacking = true;
			Shoot();
		}
		setAnimations();
		//Debug.Log(currentState.ToString());
	}

	void FixedUpdate()
	{
		isInRange = InRange(transform,direction, visionRange,"Player", Color.green);
		canShoot = InRange(transform, direction, shootingRange, "Player", Color.red);
		distanceFromPlayer = Mathf.Abs(Vector2.Distance(transform.position, player.transform.position));
	}
	#endregion

	#region Custom Functions

	void setAnimations()
	{
		anim_controller.SetBool("isAttacking", isAttacking);
	}
	/// <summary>
	/// Patrol method.
	/// </summary>
	private void Patrol()
	{
		transform.Translate(direction * Time.deltaTime * moveSpeed);
		if (Vector2.Distance(transform.position, node1.position) < .1f) {
			FlipSprite();
			facingRight = false;
			activeNode = node2;
		}
		if (Vector2.Distance(transform.position, node2.position) < .1f)
		{
			FlipSprite();
			facingRight = true;
			activeNode = node1;
		}

		if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) <= shootingRange && canShoot)
			currentState = State.Shooting;
		else if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= shootingRange
			 && Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) < visionRange && isInRange)
			currentState = State.Chasing;
	}

	private void ApproachPlayer()
	{
		transform.Translate(direction * Time.deltaTime * moveSpeedIncreased);

		if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) <= shootingRange - 0.5f && canShoot)
			currentState = State.Shooting;
		else {
			currentState = State.Patrol;
		}

	}
	private void Shoot()
	{
		//Debug.Log("I can Shoot");
		if (Time.time - lastShot > DelayBetweenBullets)
		{
			var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			
			bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * transform.localScale.x,1) * bulletSpeed * 10);
			Destroy(bullet, 2.0f);

			lastShot = Time.time;
		}


		float nodeToPlayer = Mathf.Abs(Vector2.Distance(activeNode.position, player.transform.position));
		if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= shootingRange && nodeToPlayer >= shootingRange)
			currentState = State.Patrol;
		else if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= shootingRange && isInRange)
			currentState = State.Chasing;
	}

	/// <summary>
	/// This method shoots a ray and returns if the collider hitted is the player.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="direction"></param>
	/// <param name="range"></param>
	/// <param name="objetiveTag"></param>
	/// <param name="debugColor"></param>
	/// <returns></returns>
	private bool InRange(Transform pos, Vector2 direction, float range, string objetiveTag, Color debugColor)
	{
		
		RaycastHit2D hit = Physics2D.Raycast(pos.position, direction, range);
		Debug.DrawRay(pos.position, direction * range, debugColor);

		if (hit.collider != null && hit.collider.tag == objetiveTag)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// This method simply flips the sprite localscale.
	/// </summary>
	private void FlipSprite() {
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		direction *= -1;
	}
	#endregion
}
