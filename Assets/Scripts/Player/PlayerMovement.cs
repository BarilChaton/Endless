using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Speed handling")]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float momentumDamping = 9f;
    private float playerSpeedHolder;

    // No header here. General movement.
    private CharacterController charController;
    public Animator camAnim;
    private bool isWalking;
    private Vector3 inputVector;
    private Vector3 movementVector;

    [Header("Jumping")]
    [SerializeField] private bool shouldJump = true;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    private bool canJump;

    [Header("Jumping parameters")]
    [SerializeField] private float jumpForce = 8f;

    [Header("Ground checking")]
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    const float groundCheckRadious = 0.02f;
    [SerializeField] private bool isGrounded;


    void Awake()
    {
        playerSpeedHolder = playerSpeed;
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        CheckGrounded();

        if (canJump)
        {
            HandleJump();
        }
        
        camAnim.SetBool("isWalking", isWalking);
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider[] colliders = Physics.OverlapSphere(groundCheckCollider.position, groundCheckRadious, groundLayer);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            movementVector.y *= jumpForce;

    }

    private void checkforheadbob()
    {
        if (charController.velocity.magnitude > 0.1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void MovePlayer()
    {
        charController.Move(movementVector * Time.deltaTime);
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;
        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 20f;
        }
        else
        {
            playerSpeed = playerSpeedHolder;
        }

        movementVector = (inputVector * playerSpeed) + (Vector3.up * gravity);
    }
    
}
