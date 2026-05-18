using System.Collections;
using UnityEngine;
public class ElephantCombat : MonoBehaviour
{
    [Header("General Attack Settings")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] Vector2 projectileOrigin;

    [Header("Trumpet Attack")]
    [SerializeField] bool trumpetAttack;
    [SerializeField] float trumpetAnticipationTime = 1;
    [SerializeField] float projectileAmount = 3;
    [SerializeField] float timeBetweenProjectiles = 0.2f;
    [SerializeField] Vector2 trumpetDirection;

    [Header("Stomp Attack")]
    [SerializeField] bool stompAttack;
    [SerializeField] float stompAnticipationTime = 1;

    [Header("Slash Attack")]
    [SerializeField] Transform slashOrigin;
    [SerializeField] float slashRadius;
    [SerializeField] int slashDamage;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalForce = 5f;
    [SerializeField] float slashAnticipationTime;

    [Header("Attack Detection")]
    [SerializeField] float attackHorizontalDetectRange;
    [SerializeField] float attackVerticalDetectRange;

    bool isAttacknig;
    float currentCooldown;

    [Header("References")]
    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ObjectPooling stompPool;
    [SerializeField] ElephantMovement elephantMovement;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] EnemyHealth enemyHealth;

    
    LayerMask playerLayer;
    Animator animator;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        animator=GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (CanAttack())
        {
            isAttacknig = true;
            currentCooldown = attackCooldown;
            float attackPicker = Random.Range(0, 3);
            if (attackPicker == 0) StartCoroutine(TrumpetAttack());
            else if (attackPicker == 3) StartCoroutine(StompAttack());
            else StartCoroutine(SlashAttack());
        }
        currentCooldown -= Time.deltaTime;
    }
    bool CanAttack()
    {
        Vector2 attackCapsuleSize = new Vector2(attackHorizontalDetectRange, attackVerticalDetectRange);
        bool inAttackRange = Physics2D.OverlapCapsule(transform.position, attackCapsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);

        if (elephantMovement.GetIsAlive() && currentCooldown < 0 && !knockbackScript.GetIsKnockback() && inAttackRange)
        {
            return true;
        }
        return false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.beige;

        Vector2 size = new Vector2(attackHorizontalDetectRange, attackVerticalDetectRange);
        Vector2 center = transform.position;

        float radius = attackVerticalDetectRange / 2f;
        float straightLength = attackHorizontalDetectRange - (radius * 2f);

        Vector2 left = center + Vector2.left * (straightLength / 2f);
        Vector2 right = center + Vector2.right * (straightLength / 2f);

        Gizmos.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius);
        Gizmos.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius);

        Gizmos.DrawWireSphere(left, radius);
        Gizmos.DrawWireSphere(right, radius);

        Gizmos.color = Color.bisque;

        Gizmos.DrawWireSphere(slashOrigin.position, slashRadius);
    }
   IEnumerator TrumpetAttack()
    {
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(trumpetAnticipationTime);
        for (int i=0; i<projectileAmount; i++)
        {
            trumpetPool.GetObject(projectileOrigin, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
        isAttacknig = false;
    }
    IEnumerator StompAttack()
    {
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(stompAnticipationTime);
        Debug.Log("Did stomp attack");
        isAttacknig = false;
    }
    IEnumerator SlashAttack()
    {
        yield return new WaitForSeconds(slashAnticipationTime);
        var player = Physics2D.OverlapCircle(slashOrigin.position, slashRadius, playerLayer);
        if (player != null)
        {
            Vector2 hitDirection = player.transform.position - transform.position;
            player.GetComponent<PlayerHealth>().ChangeHealth(slashDamage, hitDirection, Vector2.up, hitDirectionForce, additionalForce, player.ClosestPoint(transform.position), false);
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASHHIT);
        }
    }
    public Vector2 GetInitialDirection()
    {
        return new Vector2(trumpetDirection.x * elephantMovement.GetFacingDirection(), trumpetDirection.y);
    }
    public bool GetIsAttacking()
    {
        return isAttacknig;
    }
}