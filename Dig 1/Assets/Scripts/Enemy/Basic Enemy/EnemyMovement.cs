using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float chaseMoveSpeed = 1f;
    [SerializeField] float groundCheckLength = 1f;
    [SerializeField] float frontGroundCheckLength = 1.67f;
    [SerializeField] Vector2 frontGroundCheckOffset = new Vector2(1, 0);

    [Header("Debug")]
    [SerializeField] bool facingRight=true;
    Vector2 frontGroundCheckPos;

    Rigidbody2D enemyRB;
    Animator animator;
    EnemyState state;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
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
            frontGroundCheckPos= new Vector2(transform.position.x+frontGroundCheckOffset.x,transform.position.y+frontGroundCheckOffset.y);
            if (GetIsGroundInFront(frontGroundCheckPos))
            {
                enemyRB.linearVelocityX = idleMoveSpeed;
               
            }
            else if (GetIsGrounded())
            {
                enemyRB.linearVelocityX = -idleMoveSpeed;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingRight = false;
            }
        }
        else
        {
            frontGroundCheckPos = new Vector2(transform.position.x - frontGroundCheckOffset.x, transform.position.y - frontGroundCheckOffset.y);
            if (GetIsGroundInFront(frontGroundCheckPos))
            {
                enemyRB.linearVelocityX = -idleMoveSpeed;

            }
            else if (GetIsGrounded())
            {
                enemyRB.linearVelocityX = idleMoveSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                facingRight = true;
            }
        }
    }

    void ChasePlayer()
    {
        enemyRB.linearVelocity = new Vector2(state.GetPlayerDirection() * chaseMoveSpeed, enemyRB.linearVelocityY);
    }

    bool GetIsGroundInFront(Vector2 groundCheckPos)
    {
        return Physics2D.Raycast(groundCheckPos, Vector2.down, frontGroundCheckLength, LayerMask.GetMask("Ground"));
    }

    bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
