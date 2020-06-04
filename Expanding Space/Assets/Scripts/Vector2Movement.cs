using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2Movement : MonoBehaviour
{
    
    Rigidbody2D rigidBody;
    CapsuleCollider2D capColl;
    public Animator animator;

    Vector2 _moveDirection;

    public float speed;
    public float strafeSpeed;
    public float crouchSpeed;

    bool _isJumping;
    int _jumpCount;
    float _jumpTimeCounter;
    float _jumpForce;
    public float jumpTime;
    public float jumpForce = 20.0f;
    public int jumpCountValue = 1;

    bool _grounded;
    bool _platformGrounded;
    bool _isStanding;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask thisIsGround;
    public LayerMask thisIsPlatform;
    
    void Awake()
    {
        _jumpCount = jumpCountValue;
        rigidBody = GetComponent<Rigidbody2D>();
        capColl = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        rigidBody.velocity = new Vector2(_moveDirection.x, rigidBody.velocity.y);
    }

    void Update()
    { 
        FootingCheck();
        Jumping();
    }
    void FootingCheck()
    {
        // At the feet of the player it'll create an invisible circle equal to the groundCheckRadius that checks if the layer for thisIsGround is present. If so, it'll return true.
        _grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, thisIsGround);
        //Seperated to help with scripting that allows the player to drop through a platform.
        _platformGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, thisIsPlatform);

        //Checks if the player is standing, may it be on a fall-through platform or the ground.
        if (_grounded || _platformGrounded)
        {
            _isStanding = true; 
        } else
        {
            _isStanding = false;
        }

        //Resets the amount of mid-air-jumps the player has upon landing
        if (_isStanding)
        {
            _jumpCount = jumpCountValue;
            _jumpForce = jumpForce;
        }

        if (Input.GetButton("Down"))
        {
            Debug.Log("GET DOWN BABY!");
        }

        if (Input.GetButton("Down") && Input.GetButtonDown("Jump") && _platformGrounded)
        {
            capColl.isTrigger = true;
            Debug.Log("Dropping");
        }

        animator.SetBool("Grounded", _isStanding);
    }

    void Movement()
    {
        //Creates a new variable which will be used in the calculation
        float currentSpeed;
        
        //Sets the X value in move Direction to -1 and 1, or inbetween with dynamic inputs like a Joystick.
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(_moveDirection.x));

        //Turns around the character based on X-Input.
        if (_moveDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (_moveDirection.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }


        //Does a quick check to see if the player is currently crouched or aiborn. Will be replaced later to ensure players stay crouched under obstacles.
        if (Input.GetButton("Crouch") && _grounded){
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

        if (Input.GetButtonDown("Jump") && _isStanding && !Input.GetButton("Down") || Input.GetButtonDown("Jump") && !_isStanding && _jumpCount > 0 && !Input.GetButton("Down"))
        {
            --_jumpCount;
            _isJumping = true;
            _jumpTimeCounter = jumpTime;
            rigidBody.velocity = Vector2.up * _jumpForce;

            animator.SetBool("Jump", true);
        }
        if (Input.GetButton("Jump") && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                rigidBody.velocity = Vector2.up * _jumpForce;
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _jumpForce /= 2;
                _isJumping = false;
                animator.SetBool("Jump", false);
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            _jumpForce /= 2;
            _isJumping = false;
            animator.SetBool("Jump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        capColl.isTrigger = false;
    }
}
