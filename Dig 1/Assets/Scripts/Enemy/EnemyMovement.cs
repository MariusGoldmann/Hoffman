using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float chaseMoveSpeed = 1f;
    [SerializeField] float groundCheckLength = 1f;
    [SerializeField] Vector2 groundCheckOffset = new Vector2(1, 0);

    [Header("Debug")]
    [SerializeField] bool facingRight=true;

    Rigidbody2D rigidBody;
    Animator animator;
    EnemyState state;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        state = GetComponentInChildren<EnemyState>();
    }

    void Update()
    {
        if (state.GetInCombat()) HandleDirection();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        if (state.GetInCombat())
        {
            ChasePlayer();
        }
        else
        {
            IdleMovement();
        }
    }

    private void HandleDirection()
    {
        if (state.GetPlayerDirection() > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
        }
    }

    void HandleAnimations()
    {

    }

    void IdleMovement()
    {
        if (facingRight)
        {
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + groundCheckOffset, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")))
            {
                rigidBody.linearVelocityX = idleMoveSpeed;
            }
            else
            {
                rigidBody.linearVelocityX = -idleMoveSpeed;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingRight = false;
            }
        }
        else
        {
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) - groundCheckOffset, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")))
            {
                rigidBody.linearVelocityX = -idleMoveSpeed;
            }
            else
            {
                rigidBody.linearVelocityX = idleMoveSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                facingRight = true;
            }
        }

    }

    void ChasePlayer()
    {
        rigidBody.linearVelocity = new Vector2(state.GetPlayerDirection() * chaseMoveSpeed, rigidBody.linearVelocityY);
        
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
