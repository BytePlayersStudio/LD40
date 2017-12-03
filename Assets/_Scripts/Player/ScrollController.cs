using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour {

    #region Variables

    private Rigidbody2D _rb;
    private PlayerController _pc;
    private CriticController _cc;

    private bool _isJumping;
    private int _currentLVL;
    private bool _isWokingOut;
    private bool _isCriticizing;

    public float speed;
	private float initialSpeed;
    public float jumpForce;
	private float initialJumpForce;
	public int workOutIncrement;
    public Animator anim;

    [HideInInspector]
    public bool facingRight;

	private bool isThin;
	private bool isFat;
	private bool isSuperFat;

    #endregion

    #region Unity Functions

    void Start () {
        /*
         * Gets the RB of the player and sets private variables.
         * We are supposing that the player ALWAYS spawns facing Right
         */
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();
        _cc = GetComponent<CriticController>();

        _isJumping = false;
        _isWokingOut = false;
        facingRight = true;
        _currentLVL = 0;

		isThin = true;
		isFat = false;
		isSuperFat = false;

		initialSpeed = speed;
		initialJumpForce = jumpForce;
	}
	
	void Update () {
        /*
         * When we push down the 'Space' key, the player will jump
         */         

		if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            Jump();
        }

        anim.SetFloat("speed", Mathf.Abs(_rb.velocity.x));
        anim.SetBool("isJumping", _isJumping);
        anim.SetBool("isCriticizing", _isCriticizing);
        anim.SetBool("isWorkingOut", _isWokingOut);

		anim.SetBool("isThin", isThin);
		anim.SetBool("isFat", isFat);
		anim.SetBool("isSuperFat", isSuperFat);

		// The player reachs Lvl 2 of fatness
		if (_pc.fatness == 2 && _currentLVL != 2)
		{
			// The player cannot jump and it's too slow
			jumpForce = 0;
			speed = (initialSpeed / 3) * 2;
			_currentLVL = 2;
			changeFatnessAnimator(false, false, true);
		}
		// The player reachs Lvl 1 of fatness
		if (_pc.fatness == 1 && _currentLVL != 1)
		{
			// Velocity and jump force decreases 
			jumpForce = initialJumpForce / 2;
			speed = (initialSpeed / 3) * 2;
			_currentLVL = 1;
			changeFatnessAnimator(false, true, false);
		}
		if (_pc.fatness == 0 && _currentLVL != 0)
		{
			jumpForce = initialJumpForce;
			speed = initialSpeed ;
			_currentLVL = 0;
			changeFatnessAnimator(true, false, false);
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

        if (Input.GetKey(KeyCode.D) && !_isWokingOut)
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime, _rb.velocity.y);
            if (!facingRight)
                Flip();

        }
        else if (Input.GetKey(KeyCode.A) && !_isWokingOut)
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime * -1, _rb.velocity.y);
            if (facingRight)
                Flip();
        }
        else if (Input.GetKey(KeyCode.E) && !_isWokingOut && _pc.isCriticReady)
        {
            _cc.ShootCritic();
            StartCoroutine(DelayShoot());
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!_isWokingOut && _pc.currentFatPoints > 0)
                    StartCoroutine(WorkOut());               
            }
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
        facingRight = !facingRight;
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
	/// <summary>
	/// Sets the booleans of the animator to change states between fatness levels
	/// </summary>
	/// <param name="_isThin"></param>
	/// <param name="_isFat"></param>
	/// <param name="_isSuperFat"></param>
	private void changeFatnessAnimator(bool _isThin, bool _isFat, bool _isSuperFat)
	{
		isThin = _isThin;
		isFat = _isFat;
		isSuperFat = _isSuperFat;
	}
    #endregion

    #region Coroutines
    // This courutine, decrease currentFatPoints when doing workout
    IEnumerator WorkOut()
    {
        if (_pc.currentFatPoints >= workOutIncrement)
        {
            _isWokingOut = true;

            _pc.currentFatPoints -= workOutIncrement;
            yield return new WaitForSeconds(0.5f);
            _isWokingOut = false;
        }
        else if(_pc.currentFatPoints < workOutIncrement && _pc.currentFatPoints > 0)
        {
            _isWokingOut = true;
           
            _pc.currentFatPoints = 0;
            yield return new WaitForSeconds(0.5f);
            _isWokingOut = false;
        }
    }

    IEnumerator DelayShoot()
    {
        _isCriticizing = true;
        yield return new WaitForSeconds(0.2f);
        _isCriticizing = false;
    }

    #endregion
}
