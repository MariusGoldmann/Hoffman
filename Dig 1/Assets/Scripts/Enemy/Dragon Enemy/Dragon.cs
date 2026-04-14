using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] float idleMovespeed;
    [SerializeField] int attackCooldown;
    [SerializeField] int projectileSpeed;
    [SerializeField] int anticipationTime;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject fireProjectile;

    [Header("Raycast/Collider settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float horizontalDetectRange;
    [SerializeField] float verticalDetectRange;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;

    [Header("Layer")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;

    int facingDirection = 1;

    //Script/Component references
    KnockbackScript dragonKnockBackScript;
    Rigidbody2D dragonRB;

    Coroutine rangedAttackCoroutine;

    void Awake()
    {
        dragonKnockBackScript = GetComponent<KnockbackScript>();
        dragonRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Patrol();
        Flip();
    }

    void Patrol()
    {
        if (!IsPlayerDetected())
        {
            dragonRB.linearVelocityX = idleMovespeed * facingDirection;
            rangedAttackCoroutine = null;
        }
        else
        {
            dragonRB.linearVelocity = new(dragonRB.linearVelocity.x, dragonRB.linearVelocity.y);
            if (rangedAttackCoroutine == null)
            {
                rangedAttackCoroutine = StartCoroutine(RangedAttack());
            }
        }
    }

    IEnumerator RangedAttack()
    {
        float anticipationTimer = 0;

        while (anticipationTime > anticipationTimer)
        {
            anticipationTimer += Time.deltaTime;
            yield return null;
        }

        Vector2 fireDirection = (PlayerTarget().position - transform.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectile = Instantiate(fireProjectile, attackPoint.position, rotation);
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        while (projectile != null)
        {
            projectileRB.linearVelocity = fireDirection * projectileSpeed;
            yield return null;
        }
    }

    void Flip()
    {
        if (IsAtEdge() || IsAtWall())
        {
            facingDirection = facingDirection * -1;
        }

        if (PlayerTarget() != null)
        {
            facingDirection = (int)Mathf.Sign(PlayerTarget().position.x - transform.position.x);
        }
        transform.localScale = new Vector2(facingDirection, 1);
    }

    bool IsAtEdge()
    {
        return !Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    bool IsAtWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundLayer);
    }

    Transform PlayerTarget()
    {
        Vector2 capsuleSize = new Vector2(horizontalDetectRange, verticalDetectRange);
        Collider2D hit = Physics2D.OverlapCapsule(transform.position, capsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);
        if (hit != null)
        {
            return hit.transform;
        }
        else
        {
            return null;
        }
    }

    bool IsPlayerDetected()
    {
        Vector2 capsuleSize = new Vector2(horizontalDetectRange, verticalDetectRange);
        Collider2D hit = Physics2D.OverlapCapsule(transform.position, capsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);
        return hit;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 size = new Vector2(horizontalDetectRange, verticalDetectRange);
        Vector2 center = transform.position;

        float radius = verticalDetectRange / 2f;
        float straightLength = horizontalDetectRange - (radius * 2f);

        Vector2 left = center + Vector2.left * (straightLength / 2f);
        Vector2 right = center + Vector2.right * (straightLength / 2f);

        Gizmos.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius);
        Gizmos.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius);

        Gizmos.DrawWireSphere(left, radius);
        Gizmos.DrawWireSphere(right, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheck.position, transform.right * wallCheckDistance);
    }


}
