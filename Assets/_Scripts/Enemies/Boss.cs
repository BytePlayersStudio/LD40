/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public enum State {
		Patrol,
		AttackPlayer,
		Death
	};
	public State currentState;
	#region Variables
	//Bullet variables
	public GameObject player;
	public Transform[] shooters;
	public GameObject[] bulletPrefabs;

	public float bulletSpeed;
	private float lastShot;
	public float delayBetweenBullets = 1;
	public Transform bulletParent;
	AudioSource shotSound;

	public Transform[] waypoints;
	private int waypointID = 0;

	//Spawn Waitress Variables
	public GameObject[] waitresses;
	bool waitressesSpawned;
	//AttackPlayer
	private float lastAttack;
	public float delayBetweenAttacks = 7;
	public int secondsTochasePlayer = 4;
	public float speedIncreaseWhenAttacking = 1.2f;
	
	//Movement Variables
	public float moveSpeed = 10;
	bool goAttack =  false;
	//Stats variables
	Boss_Stats stats;
	//Animation variables
	Animator anim;
	public GameObject go_death;
	bool isThrowing = true;
	bool isAttacking = false;
	bool isDamaged =  false;

	public GameObject levelLoader;
	#endregion

	#region Unity Methods

	void Start () 
	{
		if (delayBetweenBullets == 0) delayBetweenBullets = 1.0f;
		lastShot = Time.time;
		lastAttack = Time.time;
		if (shooters == null) Debug.LogError("Assign a shooter to spawn the bullets " + this.name);
		if (player == null) Debug.LogError("Assign a player for boss: " + this.name);
		if (bulletParent == null) Debug.LogError("Assign a parent for the bullets " + this.name);
		if (waitresses == null) Debug.LogError("Assign spawning waitresses to the boss " + this.name);
		if (go_death == null) Debug.LogError("Assign death game object " + this.name);
		if (levelLoader == null) Debug.LogError("Assign levelLoader game object " + this.name);

		if (stats == null) stats = GetComponent<Boss_Stats>();

		anim = GetComponentInChildren<Animator>();
		waitressesSpawned = false;
		currentState = State.Patrol;
	}

	void Update ()
	{
		if (currentState == State.Patrol)
		{
			isThrowing = true;
			isAttacking = false;
			Patrol();
		}
		if (currentState == State.AttackPlayer)
		{
			isThrowing = false;
			isAttacking = true;
			AttackPlayer();
		}
		if (currentState == State.Death)
		{
			Death();
		}
		//Debug.Log(currentState);
		anim.SetBool("isThrowing", isThrowing);
		anim.SetBool("isAttacking", isAttacking);
		anim.SetBool("isDamaged", isDamaged);

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "CriticBullet")
		{
			isDamaged = true;
			StartCoroutine(WaitDamageAnimation(1));
			stats.health -= stats.healthDecrease;
			if (stats.health <= 0)
				currentState = State.Death;
		}
	}
	#endregion

	#region Custom Functions
	private void Death()
	{
		levelLoader.SetActive(true);
		go_death.SetActive(true);
		go_death.transform.parent = null;
		Destroy(this.transform.parent.gameObject);
	}
	private void AttackPlayer()
	{
		transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed*speedIncreaseWhenAttacking * Time.deltaTime);
		StartCoroutine(Wait(secondsTochasePlayer));
		if (goAttack == false)
		{
			lastAttack = Time.time;
			currentState = State.Patrol;
		}
	}

	private void Patrol()
	{
		if (Mathf.Abs(Vector2.Distance(waypoints[waypointID].position, transform.position)) < .5f)
		{
			changeWaypoint();
		}
		if(waitressesSpawned == false)
			SpawnWaitresses();

		Vector2 translation = Vector2.MoveTowards(transform.position, waypoints[waypointID].transform.position, moveSpeed * Time.deltaTime);
		transform.position = new Vector2(translation.x, translation.y);
		Shoot();
		if (Time.time - lastAttack > delayBetweenAttacks)
			goAttack = true;
		if (goAttack == true)
		{
			currentState = State.AttackPlayer;
		}
	}
	private void Shoot()
	{

		GameObject bulletPrf;
		bulletPrf = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
		bulletPrf.transform.localScale = new Vector3(.3f, .3f, .3f);

		if (Time.time - lastShot > delayBetweenBullets)
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
	private void SpawnWaitresses() {
		float randNum = Random.Range(0, 500);
		if (randNum == 33)
		{
			foreach (GameObject go in waitresses)
			{
				if (go.activeInHierarchy == false)
					go.SetActive(true);

			}
			waitressesSpawned = true;
		}
	}
	private void changeWaypoint()
	{
		waypointID = Random.Range(0, waypoints.Length);
	}
	IEnumerator Wait(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		goAttack = false;
	}
	IEnumerator WaitDamageAnimation(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		isDamaged = false;
	}
	#endregion
}
