using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour {

    #region Variables

    public GameObject criticBullet;
    public Transform bulletSpawn;

    public AudioClip[] shoot;

    private PlayerController _pc;
    private ScrollController _sc;
    private AudioSource _as;

    [HideInInspector]
    public bool isCriticizing;

    #endregion

    #region Unity Functions

    // Use this for initialization
    void Start () {
        _pc = GetComponent<PlayerController>();
        _sc = GetComponent<ScrollController>();
        _as = GetComponent<AudioSource>();

        isCriticizing = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.E) && _pc.isCriticReady)
        {
            isCriticizing = true;
            ShootCritic();
            isCriticizing = false;
        }
	}

    #endregion

    #region Custom Functions

    // Shoot a bullet aka Critic
    void ShootCritic()
    {
        _pc.currentCriticP = 0;

        int index = Random.Range(0, shoot.Length);
        _as.clip = shoot[index];

        var bullet = (GameObject)Instantiate(criticBullet, bulletSpawn.position, bulletSpawn.rotation);
        _as.Play();

        // Get orientation of player (-1 Left, 1 Right)
        int currentHorientation = 1;
        if (!_sc.facingRight)
            currentHorientation = -1;

        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(currentHorientation * 10 * transform.localScale.x, 1) * 400 * 10);
        Destroy(bullet, 2.0f);        
    }

    #endregion
}
