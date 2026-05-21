using Cinemachine;
using System.Collections;
using UnityEngine;
public class ElephantCombat : MonoBehaviour
{
    [Header("General Attack Settings")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float anticipationTime = 0.4f;
    [SerializeField] int collisionDamage = 5;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalForce = 5f;
    [SerializeField] Transform projectileOrigin;

    [Header("Trumpet Attack")]
    [SerializeField] bool trumpetAttack;
    [SerializeField] float projectileAmount = 3;
    [SerializeField] float timeBetweenProjectiles = 0.2f;
    [SerializeField] Vector2 trumpetDirection;

    [Header("Stomp Attack")]
    [SerializeField] float stompVerticalOffset;

    [Header("Slash Attack")]
    [SerializeField] Transform slashOrigin;
    [SerializeField] float slashRadius;
    [SerializeField] int slashDamage;
    [SerializeField] float slashAnticipationTime;

    [Header("Attack Detection")]
    [SerializeField] float attackHorizontalDetectRange;
    [SerializeField] float attackVerticalDetectRange;

    bool isAttacknig;
    float currentCooldown;

    [Header("References")]
    [SerializeField] ElephantMovement elephantMovement;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] EnemyHealth enemyHealth;

    LayerMask playerLayer;
    Animator animator;
    CinemachineImpulseSource impulseSource; 
    ObjectPooling trumpetPool;
    ObjectPooling stompPoolLeft;
    ObjectPooling stompPoolRight;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        animator=GetComponentInChildren<Animator>();
        impulseSource=GetComponent<CinemachineImpulseSource>();
        trumpetPool = GameObject.FindGameObjectWithTag("TrumpetPool").GetComponent<ObjectPooling>();
        stompPoolLeft = GameObject.FindGameObjectWithTag("StompPoolLeft").GetComponent<ObjectPooling>();
        stompPoolRight = GameObject.FindGameObjectWithTag("StompPoolRight").GetComponent<ObjectPooling>();
    }

    private void Update()
    {
        if (CanAttack(false))
        {
            isAttacknig = true;
            currentCooldown = attackCooldown;
            int attackPicker = Random.Range(0, 2);
            if (attackPicker == 0) StartCoroutine(TrumpetAttack());
            else if (attackPicker == 1) StartCoroutine(StompAttack());
            else StartCoroutine(SlashAttack());
        }
        currentCooldown -= Time.deltaTime;
    }
    public bool CanAttack(bool checkingInAttackRange)
    {
        Vector2 attackCapsuleSize = new Vector2(attackHorizontalDetectRange, attackVerticalDetectRange);
        bool inAttackRange = Physics2D.OverlapCapsule(transform.position, attackCapsuleSize, CapsuleDirection2D.Horizontal, 0f, playerLayer);
        if (checkingInAttackRange && inAttackRange) return true;
        else if (elephantMovement.GetIsAlive() && elephantMovement.GetIsGrounded() && currentCooldown < 0 && !knockbackScript.GetIsKnockback() && inAttackRange)
        {
            return true;
        }
        return false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

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

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(slashOrigin.position, slashRadius);
    }
   IEnumerator TrumpetAttack()
   {
      animator.SetTrigger("Stomp");
      yield return new WaitForSeconds(anticipationTime);
      CameraShakeManager.instance.CameraShake(impulseSource);
      for (int i=0; i<projectileAmount; i++)
      {
          GameObject projectile = trumpetPool.GetObject(projectileOrigin.position, Quaternion.identity);
          projectile.GetComponent<TrumpetShockwave>().SetInitialDirection(new Vector2(trumpetDirection.x * elephantMovement.GetFacingDirection(), trumpetDirection.y));
          yield return new WaitForSeconds(timeBetweenProjectiles);
      }
      Debug.Log("Completed ForLoop");
      isAttacknig = false;
   }
    IEnumerator StompAttack()
    {
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(anticipationTime);
        CameraShakeManager.instance.CameraShake(impulseSource);
        stompPoolLeft.GetObject(new Vector2(projectileOrigin.position.x, projectileOrigin.position.y + stompVerticalOffset), Quaternion.identity);
        stompPoolRight.GetObject(new Vector2(projectileOrigin.position.x, projectileOrigin.position.y + stompVerticalOffset), Quaternion.identity);
        isAttacknig = false;
    }
    IEnumerator SlashAttack()
    {
        yield return new WaitForSeconds(slashAnticipationTime);
        var player = Physics2D.OverlapCircle(slashOrigin.position, slashRadius, playerLayer);
        if (player != null)
        {
            Vector2 hitDirection = player.transform.position - transform.position;
            player.GetComponent<PlayerHealth>().ChangeHealth(-slashDamage, hitDirection, Vector2.up, hitDirectionForce, additionalForce, player.ClosestPoint(transform.position), false);
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASHHIT);
        }
        isAttacknig = false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))  other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-collisionDamage, other.transform.position - transform.position, Vector2.up, hitDirectionForce, additionalForce, other.GetContact(0).point, false);
    }
    public bool GetIsAttacking()
    {
        return isAttacknig;
    }
}