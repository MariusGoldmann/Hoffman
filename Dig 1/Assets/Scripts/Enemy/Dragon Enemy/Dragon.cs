using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] float idleMoveSpeed;
    [SerializeField] float recoilForce;
    [SerializeField] float detectionRadius;

    [Header("Attack settings")]
    [SerializeField] int attackCooldown;
    [SerializeField] int projectileSpeed;
    [SerializeField] int anticipationTime;
    [SerializeField] Transform attackPoint;
    [SerializeField] ObjectPooling fireProjectilePool;

    [Header("Raycast/Collider settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] GameObject player;
    [SerializeField] float horizontalDetectRange;
    [SerializeField] float verticalDetectRange;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;

    //Private variables
    float cooldownTimer;
    int facingDirection = 1;
    LayerMask groundLayer;

    //Script/Component references
    KnockbackScript dragonKnockBackScript;
    Rigidbody2D dragonRB;

    Coroutine rangedAttackCoroutine;

    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        dragonKnockBackScript = GetComponent<KnockbackScript>();
        dragonRB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Patrol();
        Flip();
        HandleCooldown();
    }

    void Patrol()
    {
        if (!IsPlayerDetected())
        {
            dragonRB.linearVelocityX = idleMoveSpeed * facingDirection;
            rangedAttackCoroutine = null;
        }
        else
        {
            dragonRB.linearVelocity = new(dragonRB.linearVelocity.x, dragonRB.linearVelocity.y);
            if (cooldownTimer <= 0)
            {
                rangedAttackCoroutine = StartCoroutine(RangedAttack());
            }
        }
    }

    IEnumerator RangedAttack()
    {
        cooldownTimer = attackCooldown;
        float anticipationTimer = 0;

        while (anticipationTime > anticipationTimer)
        {
            anticipationTimer += Time.deltaTime;
            yield return null;
        }

        if (PlayerTarget() != null)
        {
            dragonRB.linearVelocity = new Vector2((recoilForce) * facingDirection * -1, recoilForce);
            Vector2 fireDirection = (PlayerTarget().position - transform.position).normalized;
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject projectile = fireProjectilePool.GetObject(attackPoint.position, rotation);
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            while (projectile != null)
            {
                projectileRB.linearVelocity = fireDirection * projectileSpeed;
                yield return null;
            }
        }
        rangedAttackCoroutine = null;
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

    void HandleCooldown()
    {
        cooldownTimer -= Time.deltaTime;
    }

    bool IsAtEdge()
    {
        return !Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    bool IsAtWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.up, wallCheckDistance, groundLayer);
    }

    Transform PlayerTarget()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < detectionRadius)
        {
            var hit = Physics2D.Linecast(transform.position, player.transform.position, ~LayerMask.GetMask("Enemy", "FireProjectile"));

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return hit.transform;
            }
        }
        return null;
    }

    bool IsPlayerDetected()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < detectionRadius)
        {
            var hit = Physics2D.Linecast(transform.position, player.transform.position, ~LayerMask.GetMask("Enemy", "FireProjectile"));

            Debug.Log(hit.collider.gameObject.tag);

            if (hit.collider.gameObject.CompareTag("Player")) return true;

        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, player.transform.position);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheck.position, Vector2.up * wallCheckDistance);
    }


}
