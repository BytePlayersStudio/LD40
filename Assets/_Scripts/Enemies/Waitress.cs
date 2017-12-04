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
	public Transform[] waypoints;
	private int waypointID = 0;

	[HideInInspector]
	public Transform activeNode;

	//Movement variables
	public float moveSpeed;
	private float moveSpeedIncreased;
	public float speedIncreaseFactor;

	//Related to player variables.

	public GameObject player;
	Vector2 directionToPlayer;
	//Vector2 playerCurrPosition;
	//public float playerAllowedSpace;
	bool samePosition;
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
	AudioSource shotSound;

	//Animator Variables
	Animator anim_controller;
	bool isAttacking;

    //Stat Variables
    public int maxLife;
    private int _currentLife;
	#endregion

	#region Unity Methods

	void Start () 
	{
		if (shootingRange == 0) shootingRange = 4.0f;
		if (visionRange == 0) visionRange= 5.0f;

		if (moveSpeed == 0) moveSpeed = 10.0f;

		if(rb == null) rb = transform.GetComponent<Rigidbody2D>();
		if (DelayBetweenBullets == 0) DelayBetweenBullets = 1.0f;
		if (bulletParent == null) Debug.LogError("Bullet Parent not found. Set a parent for the bullets. " + this.name);

		direction = new Vector2(transform.localScale.x, 0);
		moveSpeedIncreased = moveSpeed * speedIncreaseFactor;
		samePosition = false;
		lastShot = Time.time;

		facingRight = true;
		currentState = State.Patrol;
		if (waypoints == null) {
			Debug.LogError("Assign Nodes to Waitress");
		}


		if (maxLife == 0) {
            _currentLife = 1;
            maxLife = 1;
        }
        else
        {
            _currentLife = maxLife;
        }

		anim_controller = transform.GetComponentInChildren<Animator>();
		shotSound = GetComponent<AudioSource>();
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
		if (Mathf.Abs(Vector2.Distance(waypoints[waypointID].position, transform.position)) < .5f && !samePosition)
		{
			changeWaypoint();
		}
		//Debug.Log(waipointID);
		//playerCurrPosition = new Vector2(player.transform.position.x, player.transform.position.y);
		if (!samePosition)
		{
			Vector2 translation = Vector2.MoveTowards(this.transform.position, waypoints[waypointID].transform.position, moveSpeed * Time.deltaTime);
			transform.position = new Vector2(translation.x, transform.position.y);
		}
		if (backDetection && !samePosition) {
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
		if (!samePosition)
		{
			Vector2 translation = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeedIncreased * Time.deltaTime);
			this.transform.position = new Vector2(translation.x, transform.position.y);
		}
		//Debug.Log(currentState.ToString());
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
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			//Debug.Log("OnTriggerEnter " + samePosition);
			samePosition = true;
		}

        if (collision.gameObject.tag == "CriticBullet")
        {
            Hitted();
        }
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			//Debug.Log("OnTriggerExit " + samePosition);
			samePosition = false;
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
			shotSound.Play();
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
		//Debug.DrawRay(updatedPos, direction * range, debugColor);

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
		if (waypointID == 0)
		{
			waypointID = 1;
			facingRight = false;
			FlipSprite();
		}
		else
		{
			waypointID = 0;
			facingRight = true;
			FlipSprite();
		}
	}

    private void Hitted()
    {
        --_currentLife;
        if(_currentLife <= 0 )
            Destroy(transform.parent.gameObject);
    }
	#endregion
}
