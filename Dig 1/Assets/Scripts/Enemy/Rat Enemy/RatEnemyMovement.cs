using System.Collections;
using UnityEngine;

public class RatEnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float groundCheckLength = 2.3f;

    [Header("Combat")]
    [SerializeField] int damageAmount = 5;
    [SerializeField] float chaseMoveSpeed = 2f;
    [SerializeField] float chaseDuration = 1.5f;
    [SerializeField] float chaseAnticipationTime = 0.3f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float downedCooldown = 1f;

    [Header("Knockback")]
    [SerializeField] float additionalDirectionalForce = 5f;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] KnockbackScript knockbackScript;

    [Header("Rayacst")]
    [SerializeField] float wallCheckLength = 1f;
    [SerializeField] Transform wallCheckPosition;
    [SerializeField] float frontGroundCheckLength = 1.67f;
    [SerializeField] Transform frontRaycastOrigin;

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
        Debug.Log(isCooldown);
        Debug.Log(currentCooldown);
    }
    private void FixedUpdate()
    {
        if (!isChasing && !isCooldown)
        {
            if (state.GetInCombat() && GetIsGrounded())
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
        if (!GetIsGroundInFront() || GetIsWallInFront())
        {
            StopChasePlayer(true);
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
        if (isChasing)
        {
            animator.SetBool("RatIsAggressive", true);
        }
        else
        {
            animator.SetBool("RatIsAggressive", false);
        }
        if (knockbackScript.GetIsKnockback()) animator.SetTrigger("RatKnockback");
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
    void StopChasePlayer(bool downed)
    {
        StopCoroutine(ChasePlayer());
        isChasing=false;
        if (downed)
        {
            currentCooldown = downedCooldown;
        }
        else
        {
            currentCooldown = attackCooldown;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.ChangeHealth(-damageAmount, (other.transform.position - transform.position).normalized, Vector2.up, hitDirectionForce, additionalDirectionalForce);
            StopChasePlayer(false);
        }
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
