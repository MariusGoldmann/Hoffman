using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;

    [SerializeField] float jumpForce = 35f;
    [SerializeField] float jumpCutMultiplier = 0.5f;

    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeCounter;

    [SerializeField] bool jumpPressed;// Serialized for debugging
    [SerializeField] bool jumpRelesed;



    [SerializeField] Rigidbody2D playerRB;

    private

    Vector2 moveInput;
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
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

    void FixedUpdate()
    {
        HandleMove();
        HandleJump();
    }

    void HandleMove()
    {
        playerRB.linearVelocityX = moveInput.x * walkSpeed;
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

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.8f, LayerMask.GetMask("Ground"));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.8f);
    }
}
