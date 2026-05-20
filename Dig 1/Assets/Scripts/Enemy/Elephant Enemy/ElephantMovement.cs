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
    [SerializeField] Transform groundCheckOrigin;

    [Header("Player Detection")]
    [SerializeField] float aggressiveHorizontalDetectRange;
    [SerializeField] float aggressiveVerticalDetectRange;

    bool isAlive=true;
    bool canMoveLastFrame = true;

    Rigidbody2D elephantRB;
    Animator animator;
    ElephantCombat elephantCombat;
    KnockbackScript knockbackScript;
    LayerMask playerLayer;
    LayerMask groundLayer;

    private void Start()
    {
        elephantRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        elephantCombat = GetComponent<ElephantCombat>();
        knockbackScript = GetComponent<KnockbackScript>();
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
    }
    private void Update()
    {
        if (elephantRB.linearVelocity.x !=0 && CanMove() && GetPlayerTarget()==null) animator.SetBool("IsAggressive", false);
        else if (elephantRB.linearVelocityX!=0 && CanMove()) animator.SetBool("IsAggressive", true);
    }
    private void FixedUpdate()
    {
        if (CanMove() && GetPlayerTarget() != null)
        { 
            Charge();
            Flip();
        }
        else if (CanMove())
        {
            Patrol();
            Flip();
        }
        else if (!knockbackScript.GetIsKnockback() && elephantCombat.CanAttack(true))
        {
            elephantRB.linearVelocityX = 0;
            Flip();
        }
        else if (knockbackScript.GetIsKnockback()) elephantRB.linearVelocityX = 0;
        
        if (canMoveLastFrame != CanMove())
        {
            Debug.Log("Is Alive" + isAlive);
            Debug.Log("Is Knockback" + knockbackScript.GetIsKnockback());
            Debug.Log("IsAttacking" + elephantCombat.CanAttack(true));
        }
        canMoveLastFrame = CanMove();
    }
    bool CanMove()
    {
        if (isAlive && !knockbackScript.GetIsKnockback() && !elephantCombat.CanAttack(true) && GetIsGrounded()) return true;
        return false;
    }
    void Patrol()
    {
        elephantRB.linearVelocityX = idleMoveSpeed * facingDirection;
    }
    void Charge()
    {
        elephantRB.linearVelocityX = chaseMoveSpeed * facingDirection;
    }
    void Flip()
    {
        if (!GetIsGroundInFront() || GetIsWallInFront())
        {
            facingDirection = facingDirection * -1;
            Debug.Log("Ground " + GetIsGroundInFront() + " Wall " + GetIsWallInFront());
        }
        if (GetPlayerTarget() != null)
        {
            facingDirection = (int)Mathf.Sign(GetPlayerTarget().position.x - transform.position.x);
        }
        transform.localScale = new Vector2(facingDirection, 1);
    }
    public Transform GetPlayerTarget()
    {
        Vector2 aggressiveCapsuleSize = new Vector2(aggressiveHorizontalDetectRange, aggressiveVerticalDetectRange);
        Collider2D hit = Physics2D.OverlapCapsule(transform.position, aggressiveCapsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);

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
        return Physics2D.BoxCast(frontRaycastOrigin.position, new Vector2(frontGroundCheckLength, frontGroundCheckLength), 0f, Vector2.down, frontGroundCheckLength, groundLayer);
    }
    public bool GetIsGrounded()
    {
        return Physics2D.BoxCast(groundCheckOrigin.position, new Vector2(groundCheckLength, groundCheckLength), 0f, Vector2.down, groundCheckLength, groundLayer);
    }
    bool GetIsWallInFront()
    {
        return Physics2D.Raycast(frontRaycastOrigin.position, Vector2.up, frontWallCheckLength, groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;

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

        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckOrigin.position, Vector2.down * new Vector3(groundCheckLength, groundCheckLength, groundCheckLength));

        Gizmos.color = Color.beige;
        Gizmos.DrawCube(frontRaycastOrigin.position, Vector2.down * new Vector3(groundCheckLength, groundCheckLength, groundCheckLength));

        Gizmos.color = Color.white;
        Gizmos.DrawRay(frontRaycastOrigin.position, Vector2.up * frontWallCheckLength);
    }
    public float GetFacingDirection()
    {
        return facingDirection;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }
}