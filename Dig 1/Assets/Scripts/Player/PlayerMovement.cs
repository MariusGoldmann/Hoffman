using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static playerStateMachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCutMultiplier = 0.5f;

    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeCounter;

    [Header("Bools")] // private bools
    [SerializeField] bool runPressed;// Serialized for debugging
    [SerializeField] bool jumpPressed;// Serialized for debugging
    [SerializeField] bool jumpRelesed;// Serialized for debugging
    [SerializeField] bool crouchPressed;

    [SerializeField] int facingDirection = 1;

    [SerializeField] Rigidbody2D playerRB;// Serialized for debugging
    [SerializeField] CapsuleCollider2D playerCollider;

    [Header("Inputs")]
    Vector2 moveInput;

    [Header("Script References")]
    [SerializeField] PickUpScript pickUpScript;// Serialized for debugging
    [SerializeField] playerStateMachine playerStateMachine;
    void Awake()
    { 
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        pickUpScript = GetComponent<PickUpScript>();
        playerStateMachine = GetComponent<playerStateMachine>();
    }

    void Update()
    {


        switch (playerStateMachine.movingState)
        {
            case playerStateMachine.MovingStates.Normal:
                HandleCrouch();
                break;

            case playerStateMachine.MovingStates.InAir:
                break;
           
            
        }

        Flip();
        HandleCoyoteTime();
    
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        playerRB.linearVelocityX = moveInput.x * moveSpeed;

        if (runPressed == true)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }
    void HandleJump()
    {
        if (jumpPressed == true && coyoteTimeCounter > 0)
        {
            playerRB.linearVelocityY = jumpForce;
            jumpPressed = false;
        }
        else if (jumpRelesed == true && playerRB.linearVelocityY > 0)
        {
            playerRB.linearVelocityY = (playerRB.linearVelocity.y * jumpCutMultiplier);

            coyoteTimeCounter = 0;
        }
    }

    void HandleCrouch()
    {
        if (crouchPressed == true)
        {
            playerCollider.offset = new Vector2(0, -0.58f);
            playerCollider.size = new Vector2(1, 1.86f);

            moveSpeed = crouchSpeed;
        }
        else
        {
            playerCollider.offset = new Vector2(0, -0.15f);
            playerCollider.size = new Vector2(1, 2.7f);
        }
    }

    void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            playerStateMachine.movingState = MovingStates.Normal;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            playerStateMachine.movingState = MovingStates.InAir;
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnRun(InputValue value)
    {
        if (value.isPressed && Mathf.Abs(playerRB.linearVelocityX) > 0 && pickUpScript.GetHasLeg())
        {
            runPressed = true;
        }
        else
        {
            runPressed = false;
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (coyoteTimeCounter > 0)
            {
                jumpPressed = true;
            }

            jumpRelesed = false;
        }
        else
        {
            jumpRelesed = true;
        }
    }

    void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            if (IsGrounded())
            {
                crouchPressed = true;
            }
        }
        else
        {
            crouchPressed = false;
        }
    }

    void Flip()
    {
        if (moveInput.x > 0) // Facing right
        {
            facingDirection = 1;
        }
        else if (moveInput.x < 0) // Facing left
        {
            facingDirection = -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.8f, LayerMask.GetMask("Ground"));
    }

    void OnDrawGizmos() // For debugging IsGrounded
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.8f);
    }
}
