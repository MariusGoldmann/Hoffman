using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;

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
    [SerializeField] float jumpBufferTime;
    [SerializeField] float jumpBufferCounter;

    [Header("State")]
    [SerializeField] MovingStates movingState;

    // Bools
    bool runPressed;
    bool jumpRelesed;
    bool crouchPressed;

    // Ints
    int facingDirection = 1;

    // Inputs
    Vector2 moveInput;

    //Script references
    PickUpScript pickUpScript;
    KnockbackScript knockbackScript;

    //Component references
    Rigidbody2D playerRB;
    CapsuleCollider2D playerCollider;
    Animator animator;
    void Awake()
    { 
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();

        pickUpScript = GetComponent<PickUpScript>();
        knockbackScript = FindFirstObjectByType<KnockbackScript>();
    }

    void Start()
    {
        movingState = MovingStates.OneLegIdle;
    }

    void Update()
    {
        switch (movingState)
        {
            case MovingStates.Idle:
                HandleCrouch();
                break;

            case MovingStates.OneLegIdle:
                break;

            case MovingStates.Walking:
                HandleCrouch();
                break;

            case MovingStates.OneLegWalking:
                break;

            case MovingStates.Running:
                HandleCrouch();
                break;

            case MovingStates.Jumping:
                break;

            case MovingStates.Falling:
                break;

            case MovingStates.Crouching:
                HandleCrouch();
                break;

            case MovingStates.CrouchWalking:
                HandleCrouch();
                break;

            case MovingStates.KnockBack:
                break;
        }

        Flip();
        HandleTimers();
        HandleAnimations();
        HandleStates();
    }

    void FixedUpdate()
    {
        if (!knockbackScript.GetIsKnockback())
        {
            HandleMovement();
            HandleJump();
        }
    }

    void HandleMovement()
    {
        if (runPressed)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        if (crouchPressed)
        {
            moveSpeed = crouchSpeed;
        }

        playerRB.linearVelocityX = moveInput.x * moveSpeed;

    }
    void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !crouchPressed)
        {
            playerRB.linearVelocityY = jumpForce;

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        else if (jumpRelesed && playerRB.linearVelocityY > 0)
        {
            playerRB.linearVelocityY = (playerRB.linearVelocity.y * jumpCutMultiplier);
            jumpRelesed = false;    
        }
    }

    void HandleCrouch()
    {
        if (crouchPressed)
        {
            playerCollider.offset = new Vector2(0.1f, -0.15f);
            playerCollider.size = new Vector2(1, 2.7f);
        }
        else
        {
            playerCollider.offset = new Vector2(0.1f, -0.15f);
            playerCollider.size = new Vector2(1, 2.7f);
        }
    }

    void HandleTimers()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void HandleStates()
    {
        if (IsGrounded())
        {
            animator.SetBool("IsGrounded", true);
        } 
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        if (IsGrounded() && moveInput.x == 0 && pickUpScript.GetHasLeg())
        {
            movingState = MovingStates.Idle;
        }

        if (IsGrounded() && moveInput.x == 0 && !pickUpScript.GetHasLeg())
        {
            movingState = MovingStates.OneLegIdle;
        }

        if (Mathf.Abs(moveInput.x) > 0 && pickUpScript.GetHasLeg())
        {
            movingState = MovingStates.Walking;
        }

        if (Mathf.Abs(moveInput.x) > 0 && !pickUpScript.GetHasLeg())
        {
            movingState = MovingStates.OneLegWalking;
        }

        if (Mathf.Abs(moveInput.x) > 0 && runPressed)
        {
            movingState = MovingStates.Running;
        }

        if (playerRB.linearVelocityY > 0)
        {
            movingState = MovingStates.Jumping;
        }

        if (playerRB.linearVelocityY < 0 && !IsGrounded())
        {
            movingState = MovingStates.Falling;
        }

        if (crouchPressed && IsGrounded())
        {
            movingState = MovingStates.Crouching;
        }

        if (crouchPressed && IsGrounded() && Mathf.Abs(moveInput.x) > 0 && pickUpScript.GetHasLeg())
        {
            movingState = MovingStates.CrouchWalking;
        }

        if (knockbackScript.GetIsKnockback())
        {
            movingState = MovingStates.KnockBack;
        }
    }
    void HandleAnimations()
    {
        animator.SetBool("IsWalking", movingState == MovingStates.Walking);

        animator.SetBool("OneLegWalking", movingState == MovingStates.OneLegWalking);

        animator.SetBool("IsRunning", movingState == MovingStates.Running);

        animator.SetBool("IsJumping", movingState == MovingStates.Jumping);

        animator.SetBool("IsFalling", movingState == MovingStates.Falling);

        animator.SetBool("IsCrouching", movingState == MovingStates.Crouching);

        animator.SetBool("IsCrouchWalking", movingState == MovingStates.CrouchWalking);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnRun(InputValue value)
    {
        if (value.isPressed && pickUpScript.GetHasLeg())
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

        if (value.isPressed && pickUpScript.GetHasLeg())
        {
            jumpBufferCounter = jumpBufferTime;
            jumpRelesed = false;
        }
        else
        {
            jumpRelesed = true;
        }
    }

    void OnCrouch(InputValue value)
    {
        if (value.isPressed && pickUpScript.GetHasLeg())
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

    public enum MovingStates
    {
        Idle,
        OneLegIdle,
        Walking,
        OneLegWalking,
        Running,
        Jumping,
        Falling,
        Crouching,
        CrouchWalking,
        KnockBack,
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}
