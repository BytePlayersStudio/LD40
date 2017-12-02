using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour {

    #region Variables

    public GameObject criticBullet;
    public Transform bulletSpawn;

    private PlayerController _pc;
    private ScrollController _sc;

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start () {
        _pc = GetComponent<PlayerController>();
        _sc = GetComponent<ScrollController>();
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

        // Get orientation of player (-1 Left, 1 Right)
        int currentHorientation = 1;
        if (!_sc.facingRight)
            currentHorientation = -1;

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(currentHorientation * 10 * transform.localScale.x, 1) * 400 * 10);
        Destroy(bullet, 2.0f);
    }

    #endregion
}
