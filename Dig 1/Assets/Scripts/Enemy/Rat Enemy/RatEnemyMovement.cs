using System.Collections;
using UnityEngine;

public class RatEnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float groundCheckLength = 0.1f;
    [SerializeField] float frontGroundCheckLength = 1.67f;
    [SerializeField] Transform frontRaycastOrigin;

    [Header("Combat")]
    [SerializeField] int damageAmount = 5;
    [SerializeField] float chaseMoveSpeed = 2f;
    [SerializeField] float chaseDuration = 1.5f;
    [SerializeField] float chaseAnticipationTime = 0.3f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float downedCooldown = 1f;
    [SerializeField] float wallCheckLength = 0.1f;
    [SerializeField] Transform wallCheckPosition;

    [Header("Debug")]
    [SerializeField] bool facingRight = true;
    bool isChasing;
    bool isCooldown;
    float currentCooldown;

    Rigidbody2D enemyRB;
    Animator animator;
    RatEnemyState state;
    PlayerHealth playerHealth;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        state = GetComponentInChildren<RatEnemyState>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    void Update()
    {
        HandleAnimations();
        HandleCooldowns();
    }
    private void FixedUpdate()
    {
        if (!isChasing && !isCooldown)
        {
            if (state.GetInCombat())
            {
                StartCoroutine(ChasePlayer());
            }
            else
            {
                IdleMovement();
            }
        }
    }
    void HandleCooldowns()
    {
        if (GetIsWall())
        {
            StopCoroutine(ChasePlayer());
            isChasing = false;
            currentCooldown = downedCooldown;
        }
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0)
        {
            isCooldown = false;
        }
        else
        {
            isCooldown = true;
        }
    }
    void HandleAnimations()
    {

    }
    void IdleMovement()
    {
        if (facingRight && GetIsGrounded())
        {
            if (!GetIsGroundInFront() || GetIsWallInFront())
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingRight = false;
            }
            else
            {
                enemyRB.linearVelocityX = idleMoveSpeed;
            }
        }
        else if (GetIsGrounded())
        {
            if (!GetIsGroundInFront() || GetIsWallInFront())
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                facingRight = true;
            }
            else
            {
                enemyRB.linearVelocityX = -idleMoveSpeed;

            }
            
        }
    }
    IEnumerator ChasePlayer()
    {
        isChasing = true;
        float chaseTime=0f;

        enemyRB.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(chaseAnticipationTime);

        while (chaseTime<chaseDuration)
        {
            if (facingRight)
            { 
                enemyRB.linearVelocity = new Vector2(chaseMoveSpeed, enemyRB.linearVelocityY);
            }
            else
            {
                enemyRB.linearVelocity = new Vector2(-chaseMoveSpeed, enemyRB.linearVelocityY);
            }
            chaseTime += Time.fixedDeltaTime;
            yield return null;
        }

        isChasing = false;
        currentCooldown = attackCooldown;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) playerHealth.ChangeHealth(-damageAmount, (other.transform.position-transform.position).normalized, Vector2.up);
    }
    bool GetIsGroundInFront()
    {
        return Physics2D.Raycast(frontRaycastOrigin.position, Vector2.down, frontGroundCheckLength, LayerMask.GetMask("Ground"));
    }
    bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));
    }
    bool GetIsWallInFront()
    {
        return Physics2D.Raycast(frontRaycastOrigin.position, Vector2.right, groundCheckLength, LayerMask.GetMask("Ground"));
    }
    bool GetIsWall()
    {
        return Physics2D.Raycast(wallCheckPosition.position, Vector2.right, wallCheckLength, LayerMask.GetMask("Ground"));
    }
    public bool GetFacingRight()
    {
        return facingRight;
    }
    public bool GetIsChasing()
    {
        return isChasing;
    }
    public bool GetIsCooldown()
    {
        return isCooldown;
    }
}
