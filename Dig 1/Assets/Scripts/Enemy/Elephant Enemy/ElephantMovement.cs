using UnityEngine;

public class ElephantMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float idleMoveSpeed = 2f;
    [SerializeField] float chaseMoveSpeed = 5f;
    [SerializeField] float facingDirection = 1;
    [SerializeField] KnockbackScript elephantKnockbackScript;

    [Header("Rayacst")]
    [SerializeField] float groundCheckLength = 2.3f;
    [SerializeField] float frontGroundCheckLength = 0.2f;
    [SerializeField] float frontWallCheckLength = 4f; //Elepthant Heigth
    [SerializeField] Transform frontRaycastOrigin;

    [Header("Player Detection")]
    [SerializeField] float aggressiveHorizontalDetectRange;
    [SerializeField] float aggressiveVerticalDetectRange;
    [SerializeField] float attackHorizontalDetectRange;
    [SerializeField] float attackVerticalDetectRange;


    [Header("Debug")]
    [SerializeField] bool knockedOut;
    bool aggressive;
    bool inAttackRange;

    Rigidbody2D elephantRB;
    Animator animator;
    EnemyHealth enemyHealth;
    ElephantCombat elephantCombat;
    LayerMask playerLayer;
    LayerMask groundLayer;

    private void Start()
    {
        elephantRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        elephantCombat = GetComponent<ElephantCombat>();
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
    }
    void Update()
    {
        HandleAnimations();
    }
    void HandleAnimations()
    {

    }
    private void FixedUpdate()
    {
        if (!knockedOut && GetIsGrounded() && PlayerTarget() == null)
        {
            Patrol();
            Flip();
        }
        else if (!knockedOut && GetIsGrounded())
        {
            Charge();
            Flip();
        }
    }
    void Patrol()
    {
        elephantRB.linearVelocityX = idleMoveSpeed * facingDirection;
    }
    void Charge()
    {
        if (!inAttackRange) elephantRB.linearVelocityX = chaseMoveSpeed * facingDirection;
        else elephantCombat.trumpetCoroutine = StartCoroutine(elephantCombat.TrumpetAttack()); // Random attack logic?
    }
    void Flip()
    {
        if (!GetIsGroundInFront() || GetIsWallInFront())
        {
            facingDirection = facingDirection * -1;
        }
        if (PlayerTarget() != null)
        {
            facingDirection = (int)Mathf.Sign(PlayerTarget().position.x - transform.position.x);
        }
        transform.localScale = new Vector2(facingDirection, 1);
    }
    Transform PlayerTarget()
    {
        Vector2 aggressiveCapsuleSize = new Vector2(aggressiveHorizontalDetectRange, aggressiveVerticalDetectRange);
        Vector2 attackCapsuleSize = new Vector2(attackHorizontalDetectRange, attackVerticalDetectRange);
        Collider2D hit = Physics2D.OverlapCapsule(transform.position, aggressiveCapsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);
        inAttackRange = Physics2D.OverlapCapsule(transform.position, attackCapsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);

        if (hit != null)
        {
            return hit.transform;
        }
        else
        {
            return null;
        }
    }
    bool GetIsGroundInFront()
    {
        return Physics2D.Raycast(frontRaycastOrigin.position, Vector2.down, frontGroundCheckLength, groundLayer);
    }
    bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, groundLayer);
    }
    bool GetIsWallInFront()
    {
        return Physics2D.Raycast(frontRaycastOrigin.position, Vector2.up, frontWallCheckLength, groundLayer);
    }
    public void SetKnockedOut(bool b)
    {
        knockedOut = b;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 size = new Vector2(aggressiveHorizontalDetectRange, aggressiveVerticalDetectRange);
        Vector2 center = transform.position;

        float radius = aggressiveVerticalDetectRange / 2f;
        float straightLength = aggressiveHorizontalDetectRange - (radius * 2f);

        Vector2 left = center + Vector2.left * (straightLength / 2f);
        Vector2 right = center + Vector2.right * (straightLength / 2f);

        Gizmos.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius);
        Gizmos.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius);

        Gizmos.DrawWireSphere(left, radius);
        Gizmos.DrawWireSphere(right, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckLength);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(frontRaycastOrigin.position, Vector2.down * frontGroundCheckLength);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(frontRaycastOrigin.position, Vector2.up * frontWallCheckLength);
    }
    public float GetFacingDirection()
    {
        return facingDirection;
    }
}