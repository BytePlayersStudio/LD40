using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour {

    #region Variables

    public GameObject criticBullet;
    public Transform bulletSpawn;

    private PlayerController _pc;    

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start () {
        _pc = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.E) && _pc.isCriticReady)
        {
            ShootCritic();
        }
	}

    #endregion

    #region Custom Functions

    // Shoot a bullet aka Critic
    void ShootCritic()
    {   
        _pc.currentCriticP = 0;
        var bullet = (GameObject)Instantiate(criticBullet, bulletSpawn.position, bulletSpawn.rotation);

        // Get orientation of player

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * transform.localScale.x, 1) * 400 * 10);
        Destroy(bullet, 2.0f);
    }

    #endregion
}
