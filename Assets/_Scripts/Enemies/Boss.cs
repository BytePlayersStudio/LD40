/*
* Copyright (c) Ignacio Martinez
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	#region Variables
	public Transform[] bulletSpawns;

	public GameObject[] bulletPrefabs;
	public float bulletSpeed;
	private float lastShot;
	public float DelayBetweenBullets = 1;
	public Transform bulletParent;
	AudioSource shotSound;


	#endregion

	#region Unity Methods

	void Start () 
	{
		if (DelayBetweenBullets == 0) DelayBetweenBullets = 1.0f;
		if (bulletParent == null) Debug.LogError("Bullet Parent not found. Set a parent for the bullets.");
		lastShot = Time.time;
	}

	void Update ()
	{
		Shoot();
	}
	#endregion

	#region Custom Functions

	private void Shoot()
	{

		GameObject bulletPrf;
		bulletPrf = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
		bulletPrf.transform.localScale = new Vector3(.3f, .3f, .3f);

		if (Time.time - lastShot > DelayBetweenBullets)
		{
			foreach (Transform t in bulletSpawns)
			{
				var bullet = (GameObject)Instantiate(bulletPrf, t.position, t.rotation);
				bullet.transform.SetParent(bulletParent);
				bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * t.localScale.x, 1) * bulletSpeed * 10);
				Destroy(bullet, 2.0f);
			}
			//shotSound.Play();
			lastShot = Time.time;
		}


		//float nodeToPlayer = Mathf.Abs(Vector2.Distance(activeNode.position, player.transform.position));
		/*if (!isInRange && !canShoot)
			currentState = State.Patrol;
		if (isInRange && !canShoot)
			currentState = State.Chasing;*/

	}
	#endregion
}
