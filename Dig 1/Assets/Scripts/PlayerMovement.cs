using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCutMultiplier = 0.5f;

    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeCounter;

    [Header("Bools")] // private bools
    [SerializeField] bool runPressed;// Serialized for debugging
    [SerializeField] bool jumpPressed;// Serialized for debugging
    [SerializeField] bool jumpRelesed;// Serialized for debugging

    [SerializeField] int facingDirection = 1;

    [SerializeField] Rigidbody2D playerRB;// Serialized for debugging

    [Header("Inputs")]
    Vector2 moveInput;


    [Header("Script References")]
    [SerializeField] PickUpScript pickUpScript;// Serialized for debugging
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();

        pickUpScript = GetComponent<PickUpScript>();
    }

    void Update()
    {
        HandleCoyoteTime();
        Flip();
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

    void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
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
        return Physics2D.Raycast(transform.position, Vector2.down, 0.8f, LayerMask.GetMask("Ground"));
    }

    void OnDrawGizmos() // For debugging IsGrounded
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.8f);
    }
}
