using Cinemachine;
using System.Collections;
using UnityEngine;

public class ElephantMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float idleMoveSpeed = 2f;
    [SerializeField] float chaseMoveSpeed = 5f;
    [SerializeField] float facingDirection = 1;
    [SerializeField] KnockbackScript elephantKnockbackScript;

    [Header("Rayacst")]
    [SerializeField] float groundCheckLength;
    [SerializeField] float frontGroundCheckLength;
    [SerializeField] float frontWallCheckLength;
    [SerializeField] Transform frontRaycastOrigin;
    [SerializeField] Transform groundCheckOrigin;

    [Header("Player Detection")]
    [SerializeField] float aggressiveHorizontalDetectRange;
    [SerializeField] float aggressiveVerticalDetectRange;

    [Header("Death Sequence")]
    [SerializeField] GameObject hpParticlePrefab;
    [SerializeField] ParticleSystem hitParticles;

    bool isAlive = true;
    bool groundedLastFrame = false;

    Rigidbody2D elephantRB;
    Animator animator;
    ElephantCombat elephantCombat;
    KnockbackScript knockbackScript;
    CinemachineImpulseSource impulseSource;
    LayerMask playerLayer;
    LayerMask groundLayer;

    private void Start()
    {
        elephantRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        elephantCombat = GetComponent<ElephantCombat>();
        knockbackScript = GetComponent<KnockbackScript>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
    }
    private void Update()
    {
        if (CanMove() && GetPlayerTarget()!=null) animator.SetBool("IsAggressive", true);
        else animator.SetBool("IsAggressive", false);

        if (elephantRB.linearVelocityX == 0) animator.SetBool("IsIdle", true);
        else animator.SetBool("IsIdle", false);

        if (!isAlive) animator.SetTrigger("IsDead");
        else if (knockbackScript.GetIsKnockback()) animator.SetTrigger("IsKnockback");
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
        if (GetIsGrounded() && !groundedLastFrame) CameraShakeManager.instance.CameraShake(impulseSource);
        groundedLastFrame = GetIsGrounded();
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
        }
        if (GetPlayerTarget() != null)
        {
            facingDirection = (int)Mathf.Sign(GetPlayerTarget().position.x - transform.position.x);
        }
        transform.localScale = new Vector2(facingDirection, 1);
    }
    public IEnumerator DeathSequence()
    {
        float hpParticleAmount = Random.Range(1, 4);
        isAlive = false;
        yield return new WaitForSeconds(0.55f-0.1f*hpParticleAmount);
        for (int i = 0; i < hpParticleAmount; i++)
        {
            Instantiate(hpParticlePrefab, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
            hitParticles.transform.position = transform.position;
            hitParticles.Play();
            CameraShakeManager.instance.CameraShake(impulseSource);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
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
    public float GetFacingDirection()
    {
        return facingDirection;
    }

    public bool GetIsAlive()
    {
        return isAlive;
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
}