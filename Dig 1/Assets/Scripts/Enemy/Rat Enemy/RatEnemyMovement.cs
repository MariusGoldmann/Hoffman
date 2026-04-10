using System.Collections;
using UnityEngine;

public class RatEnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float idleMoveSpeed = 2f;
    [SerializeField] float groundCheckLength = 2.3f;

    [Header("Combat")]
    [SerializeField] int damageAmount = 5;
    [SerializeField] float chaseMoveSpeed = 5f;
    [SerializeField] float chaseDuration = 8f;
    [SerializeField] float chaseAnticipationTime = 1f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float dazedCooldown = 3f;

    [Header("Knockback")]
    [SerializeField] float additionalForce = 10f;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] KnockbackScript ratKnockbackScript;

    [Header("Rayacst")]
    [SerializeField] float wallCheckLength = 1f;
    [SerializeField] Transform wallCheckPosition;
    [SerializeField] float frontGroundCheckLength = 1.67f;
    [SerializeField] Transform frontRaycastOrigin;

    [Header("Debug")]
    [SerializeField] bool facingRight = true;
    [SerializeField] bool isChasing;
    [SerializeField] bool isCooldown;
    [SerializeField] float currentCooldown;

    [SerializeField] bool knockedOut;

    Coroutine chasePlayerCoroutine;

    Rigidbody2D ratRB;
    Animator animator;
    RatEnemyState ratEnemyState;
    PlayerHealth playerHealth;

    private void Start()
    {
        ratRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        ratEnemyState = GetComponentInChildren<RatEnemyState>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    void Update()
    {
        HandleAnimations();
        HandleCooldowns();
    }
    private void FixedUpdate()
    {
        if (!isChasing && !isCooldown && !knockedOut && !ratKnockbackScript.GetIsKnockback())
        {
            if (ratEnemyState.GetInCombat() && GetIsGrounded())
            {
                if (chasePlayerCoroutine==null) chasePlayerCoroutine=StartCoroutine(ChasePlayer());
            }
            else
            {
                if (chasePlayerCoroutine != null) StopChasePlayer(false);
                IdleMovement();
            }
        }

        if (knockedOut)
        {
            ratRB.linearVelocity = Vector2.zero;
        }
        Debug.Log(isCooldown);
    }
    void HandleCooldowns()
    {
        currentCooldown -= Time.deltaTime;

        if (isChasing && (!GetIsGroundInFront() || GetIsWallInFront()))
        {
            StopChasePlayer(true);
        }
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
        if (ratKnockbackScript.GetIsKnockback()) animator.SetTrigger("RatKnockback");
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
                ratRB.linearVelocityX = idleMoveSpeed;
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
                ratRB.linearVelocityX = -idleMoveSpeed;
            }
        }
    }
    IEnumerator ChasePlayer()
    {
        isChasing = true;
        float chaseTime=0f;

        ratRB.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(chaseAnticipationTime);

        while (chaseTime<chaseDuration && !ratKnockbackScript.GetIsKnockback())
        {
            if (facingRight)
            { 
                ratRB.linearVelocity = new Vector2(chaseMoveSpeed, ratRB.linearVelocityY);
            }
            else
            {
                ratRB.linearVelocity = new Vector2(-chaseMoveSpeed, ratRB.linearVelocityY);
            }
            chaseTime += Time.fixedDeltaTime;
            yield return null;
        }

        isChasing = false;
        currentCooldown = attackCooldown;
    }
    void StopChasePlayer(bool dazed)
    {
        StopCoroutine(chasePlayerCoroutine);
        isChasing=false;
        if (dazed)
        {
            currentCooldown = dazedCooldown;
        }
        else
        {
            currentCooldown = attackCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && isChasing)
        {
            playerHealth.ChangeHealth(-damageAmount, (other.transform.position - transform.position).normalized, Vector2.up, hitDirectionForce, additionalForce);
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

    public void SetKnockedOut(bool b)
    {
        knockedOut = b;
    }
}
