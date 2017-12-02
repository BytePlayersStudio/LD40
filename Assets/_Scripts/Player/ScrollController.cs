using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour {

    private Rigidbody2D _rb;
    private bool _isJumping;
    private bool _facingRight;

    public float speed;
    public float jumpForce;
    
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _isJumping = false;
        _facingRight = true;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            Jump();
        }
	}

    private void FixedUpdate()
    {
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

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f);
        _rb.AddForce(new Vector2(0f, jumpForce));

        _isJumping = true;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            _isJumping = false;
        }
    }
}
