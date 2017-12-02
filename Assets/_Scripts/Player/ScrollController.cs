using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour {

    private Rigidbody2D _rb;
    private bool _isJumping;

    public float speed;
    public float jumpForce;
	Transform groundCheck;
    
	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _isJumping = false;
		groundCheck = transform.GetChild(1).transform;
	}
	
	// Update is called once per frame
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
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(speed * Time.deltaTime * -1, _rb.velocity.y);
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f);
        _rb.AddForce(new Vector2(0f, jumpForce));
        _isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            _isJumping = false;
    }



}
