using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float oneLegSpeed;
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

    public bool isOnPlatform;

    // Ints
    int facingDirection = 1;

    // Inputs
    Vector2 moveInput;

    //Script references
    [SerializeField] PickUpScript pickUpScript;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] DialogueController dialogueController;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] MovingPlatform movingPlatform;
    [SerializeField] PlayerHealth playerHealth;

    //Component references
    Rigidbody2D        playerRB;
    public Rigidbody2D platformRB;
    CapsuleCollider2D  playerCollider;
    Animator           animator;
    public Rigidbody2D platformRb;

    void Awake()
    { 
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();

        pickUpScript       = GetComponent<PickUpScript>();
        knockbackScript    = GetComponent<KnockbackScript>();
        spawnManager       = FindFirstObjectByType<SpawnManager>();
        playerHealth       = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        movingState        = MovingStates.OneLegIdle;
        transform.position = SpawnManager.instance.spawnPosition;
    }

    void Update()
    {
	    if (Keyboard.current.eKey.wasPressedThisFrame) {
		    animator.SetTrigger("StandUp");
	    }
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
        
    }

    void FixedUpdate()
    {
        if (!dialogueController.GetIsInDialogue() && !knockbackScript.GetIsKnockback() && !playerHealth.GetIsDead())
        {
            HandleMovement();
            HandleJump();
        }
        else if (dialogueController.GetIsInDialogue() || playerHealth.GetIsDead())
        {
            playerRB.linearVelocity = new Vector2(0, playerRB.linearVelocityY);
            moveInput.x = 0;
        }

        HandleStates();
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

        if (!pickUpScript.GetHasLeg())
        {
            moveSpeed = oneLegSpeed;
        }

        if (isOnPlatform)
        {
            playerRB.linearVelocityX = (moveInput.x * moveSpeed);// + platformRB.linearVelocityX;
        }
        else
        {
            playerRB.linearVelocityX = moveInput.x * moveSpeed;
        }

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

        if (IsGrounded() && moveInput.x == 0 && pickUpScript.GetHasLeg() || dialogueController.GetIsInDialogue())
        {
            movingState = MovingStates.Idle;
        }

        if (IsGrounded() && moveInput.x == 0 && !pickUpScript.GetHasLeg() || dialogueController.GetIsInDialogue())
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

        if (playerRB.linearVelocityY > 0.5f && !IsGrounded())
        {
            movingState = MovingStates.Jumping;
        }

        if (playerRB.linearVelocityY < -0.8f && !IsGrounded())
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

        if (playerHealth.GetIsDead())
        {
            movingState = MovingStates.Dead;
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

        animator.SetBool("Knockback", movingState == MovingStates.KnockBack);
    }

    void OnMove(InputValue value)
    {
        if (dialogueController.GetIsInDialogue() == false)
        {
            moveInput = value.Get<Vector2>();
        }
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
        Vector2 boxSize = new Vector2(playerCollider.size.x, playerCollider.size.y * 0.5f);
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(0, -playerCollider.size.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.down, 0.8f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
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
        Dead
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
