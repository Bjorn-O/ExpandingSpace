using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6f;
    public float airSpeed = 4f;
    public float jumpSpeed = 8f;
    public float doubleJumpSpeed = 4f;
    public float gravity = 20.0f;

    private bool _doubleJump;
    private Vector3 _moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            

            _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
            _moveDirection *= speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump!");
                _moveDirection.y = jumpSpeed;
                _doubleJump = true;
                Debug.Log(_doubleJump);
            }
            //_moveDirection *= airSpeed;
            if (Input.GetKeyDown(KeyCode.Space) && !characterController.isGrounded)
            {
                Debug.Log("I'm Jumping TWICE!");
                _moveDirection.y += doubleJumpSpeed;
                _doubleJump = false;
            }
        }

        _moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(_moveDirection * Time.deltaTime);
    }
}
