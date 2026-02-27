using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float chaseMoveSpeed = 1f;
    [SerializeField] float groundCheckLength = 1.67f;
    [SerializeField] Vector2 groundCheckOffset = new Vector2(1, 0);

    [Header("Debug")]
    [SerializeField] bool facingRight=true;
    Vector2 groundCheckPos;

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
            groundCheckPos= new Vector2(transform.position.x+groundCheckOffset.x,transform.position.y+groundCheckOffset.y);
            if (Physics2D.Raycast(groundCheckPos, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")))
            {
                enemyRB.linearVelocityX = idleMoveSpeed;
               
            }
            else
            {
                enemyRB.linearVelocityX = -idleMoveSpeed;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingRight = false;
            }
        }
        else
        {
            groundCheckPos = new Vector2(transform.position.x - groundCheckOffset.x, transform.position.y - groundCheckOffset.y);
            if (Physics2D.Raycast(groundCheckPos, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")))
            {
                enemyRB.linearVelocityX = -idleMoveSpeed;

            }
            else
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

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
