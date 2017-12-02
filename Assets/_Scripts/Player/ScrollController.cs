using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour {

    #region Variables

    private Rigidbody2D _rb;
    private PlayerController _pc;
    private bool _isJumping;
    private bool _facingRight;
    private int _currentLVL;

    public float speed;
    public float jumpForce;

    #endregion

    #region Unity Functions

    void Start () {
        /*
         * Gets the RB of the player and sets private variables.
         * We are supposing that the player ALWAYS spawns facing Right
         */
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();
        _isJumping = false;
        _facingRight = true;
        _currentLVL = 0;
	}
	
	void Update () {
        /*
         * When we push down the 'Space' key, the player will jump
         */
		if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            Jump();
        }

        // The player reachs Lvl 2 of fatness
        if (_pc.fatness == 2 && _currentLVL != 2)
        {
            // The player cannot jump and it's too slow
            jumpForce = 0;
            speed = (speed / 3) * 2;
            _currentLVL = 2;
        }
        // The player reachs Lvl 1 of fatness
        else if (_pc.fatness == 1 && _currentLVL != 1)
        {
            // Velocity and jump force decreases 
            jumpForce /= 2;
            speed = (speed / 3) * 2;
            _currentLVL = 1;
        }

    }

    private void FixedUpdate()
    {
        /*
         * If the player is clicking D, will go right.
         * If the player is clicking A, will go left.
         * If he is not clicking any of those keys, will stay idle.
         * 
         * We are always checking player's orientation
         */
        if (Input.GetKey(KeyCode.D))
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime, _rb.velocity.y);
            if (!_facingRight)
                Flip();

        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime * -1, _rb.velocity.y);
            if (_facingRight)
                Flip();
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    #endregion

    #region Custom Functions

    // Calculates the Jump of the Player
    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f);
        _rb.AddForce(new Vector2(0f, jumpForce));

        _isJumping = true;
    }

    // Flips the Player when he tries to change his direction
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0, Space.Self);
    }

    // Detects if the player is colliding with the floor
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            _isJumping = false;
        }
    }

    #endregion
}
