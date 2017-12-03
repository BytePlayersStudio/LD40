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
	public float backDetectionRange;
	Vector2 direction;

	//Nodes for the patrol behavior
	public Transform[] waipoints;
	private int waipointID = 0;

	[HideInInspector]
	public Transform activeNode;

	//Movement variables
	public float moveSpeed;
	private float moveSpeedIncreased;
	public float speedIncreaseFactor;

	//Related to player variables.

	public GameObject player;
	Vector2 directionToPlayer;

	//Detection variables

	private bool isInRange;
	private bool canShoot;
	public bool backDetection;


	private bool facingRight;
	//Shooting mechanics
	public Transform bulletParent;
	public GameObject[] bulletPrefabs;
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

		if (moveSpeed == 0) moveSpeed = 10.0f;

		if(rb == null) rb = transform.GetComponent<Rigidbody2D>();
		if (DelayBetweenBullets == 0) DelayBetweenBullets = 1.0f;
		if (bulletParent == null) Debug.LogError("Bullet Parent not found. Set a parent for the bullets.");

		direction = new Vector2(transform.localScale.x, 0);
		moveSpeedIncreased = moveSpeed * speedIncreaseFactor;

		lastShot = Time.time;

		facingRight = true;
		currentState = State.Patrol;

		anim_controller = transform.GetComponentInChildren<Animator>();

		//Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}

	void Update ()
	{
		if (currentState == State.Patrol)
		{
			isAttacking = false;
			Patrol();
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
		backDetection = InRange(transform, -direction, backDetectionRange, "Player", Color.blue);

	}

	#endregion

	#region Custom Functions
	/// <summary>
	/// Set the animation variables.
	/// </summary>
	void setAnimations()
	{
		anim_controller.SetBool("isAttacking", isAttacking);
	}
	/// <summary>
	/// Patrol method.
	/// </summary>
	private void Patrol()
	{
		if (Mathf.Abs(Vector2.Distance(waipoints[waipointID].position, transform.position)) < .5f)
		{
			changeWaypoint();
		}
		//Debug.Log(waipointID);
		Vector2 translation = Vector2.MoveTowards(this.transform.position, waipoints[waipointID].transform.position, moveSpeed * Time.deltaTime);
		transform.position = new Vector2(translation.x, transform.position.y); ;

		if (backDetection) {
			changeWaypoint();
			currentState = State.Chasing;
		}
		if (canShoot)
			currentState = State.Shooting;
		//if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= shootingRange
		//	 && Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) < visionRange && isInRange && !canShoot)
		if(!canShoot && isInRange)
			currentState = State.Chasing;
	}
	/// <summary>
	/// Approach Player
	/// </summary>
	private void ApproachPlayer()
	{
		Vector2 translation = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeedIncreased * Time.deltaTime);
		this.transform.position = new Vector2(translation.x, transform.position.y);

		//if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) <= shootingRange  || canShoot)
		//	currentState = State.Shooting;
		//if(Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) > shootingRange || !canShoot)
		//	currentState = State.Patrol;
		if (canShoot) currentState = State.Shooting;
		if (!isInRange)
		{
			currentState = State.Patrol;
		}

	}
	/// <summary>
	/// Makes the enemy shoot
	/// </summary>
	private void Shoot()
	{
		//Debug.Log("I can Shoot");
		GameObject bulletPrf;
		bulletPrf = bulletPrefabs[Random.Range(0,bulletPrefabs.Length)];
		if (Time.time - lastShot > DelayBetweenBullets)
		{
			var bullet = (GameObject)Instantiate(bulletPrf, bulletSpawn.position, bulletSpawn.rotation);
			bullet.transform.SetParent(bulletParent);
			bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * transform.localScale.x,1) * bulletSpeed * 10);
			Destroy(bullet, 2.0f);

			lastShot = Time.time;
		}


		//float nodeToPlayer = Mathf.Abs(Vector2.Distance(activeNode.position, player.transform.position));
		if (!isInRange && !canShoot)
			currentState = State.Patrol;
		if (isInRange && !canShoot)
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
		Vector3 updatedPos;
		//Debug.Log("facing right is:" + facingRight);
		if (facingRight)
		{
			updatedPos = new Vector3(pos.position.x + 0.5f, pos.position.y, pos.position.z);
		}
		else
		{
			updatedPos = new Vector3(pos.position.x - 0.5f, pos.position.y, pos.position.z);
		}

		RaycastHit2D hit = Physics2D.Raycast(updatedPos, direction, range);
		Debug.DrawRay(updatedPos, direction * range, debugColor);

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

	private void changeWaypoint() {
		if (waipointID == 0)
		{
			waipointID = 1;
			facingRight = false;
			FlipSprite();
		}
		else
		{
			waipointID = 0;
			facingRight = true;
			FlipSprite();
		}
	}
	#endregion
}
