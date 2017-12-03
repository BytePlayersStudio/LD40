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
    public float jumpForce;
    public int workOutIncrement;
    public Animator anim;

    [HideInInspector]
    public bool facingRight;

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
	}
	
	void Update () {
        /*
         * When we push down the 'Space' key, the player will jump
         */

        _cc.isCriticizing = _isCriticizing;

		if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            Jump();
        }

        anim.SetFloat("speed", Mathf.Abs(_rb.velocity.x));
        anim.SetBool("isJumping", _isJumping);
        anim.SetBool("isCriticizing", _isCriticizing);
        anim.SetBool("isWorkingOut", _isWokingOut);

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
            if (!facingRight)
                Flip();

        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime * -1, _rb.velocity.y);
            if (facingRight)
                Flip();
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!_isWokingOut)
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

    #endregion

    #region Coroutines
    // This courutine, decrease currentFatPoints when doing workout
    IEnumerator WorkOut()
    {
        if (_pc.currentFatPoints >= workOutIncrement)
        {
            _isWokingOut = true;
                
            // Start WorkOut Animation & block player

            _pc.currentFatPoints -= workOutIncrement;
            yield return new WaitForSeconds(1);
            _isWokingOut = false;
        }
        else if(_pc.currentFatPoints < workOutIncrement && _pc.currentFatPoints > 0)
        {
            _isWokingOut = true;

            // Block player

            _pc.currentFatPoints = 0;
            yield return new WaitForSeconds(1);
            _isWokingOut = false;
        }
    }

    #endregion
}
