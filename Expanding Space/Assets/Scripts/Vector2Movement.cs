using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2Movement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private Vector2 _moveDirection;

    private float speed = 10.0f;
    private float jumpForce = 20.0f;
    private float strafeSpeed = 5.0f;
    private float crouchSpeed = 6.0f;
    private int jumpCount;
    public int jumpCountValue;

    private bool _grounded;

    void Awake()
    {
        jumpCount = jumpCountValue;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Walking();
        
        rigidBody.velocity = new Vector2(_moveDirection.x, rigidBody.velocity.y);
    }

    void Update()
    {
        Jumping();
        if (_grounded)
        {
            jumpCount = jumpCountValue;
        }
    }

    void Walking()
    {
        //Creates a new variable which will be used in the calculation
        float currentSpeed;
        //Sets the X value in move Direction to -1 and 1, or inbetween with dynamic inputs like a Joystick.
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        //Does a quick check to see if the player is currently crouched or aiborn. Will be replaced later to ensure players stay crouched under obstacles.
        if (Input.GetButton("Down") && _grounded){
            currentSpeed = crouchSpeed;
        }
        else if (!_grounded)
        {
            currentSpeed = strafeSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
        // Multiplys the -1 to 1 with the speed depicted if you're crouching or not.
        _moveDirection.x *= currentSpeed;
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            rigidBody.velocity = Vector2.up * jumpForce;
        }
        else if (Input.GetButtonDown("Jump") && !_grounded && jumpCount != 0)
        {
            rigidBody.velocity = Vector2.up * jumpForce;
            --jumpCount; 
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        _grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _grounded = false;  
    }
}
