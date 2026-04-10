using Cinemachine;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;

    [SerializeField] float timeDead=3f;


    [SerializeField] bool knockedOut = false;

    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] DamageFlash damageFlash;
    [SerializeField] KnockbackScript enemyKnockbackScript;
    [SerializeField] BlobfishCombat blobfishCombat;
    [SerializeField] RatEnemyMovement ratEnemyMovement;

    [SerializeField] ParticleSystem hitParticles;

    CinemachineImpulseSource impulseSource;


    SpriteRenderer spriteRenderer;

    Animator animator;

    void Awake()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        enemyKnockbackScript = GetComponent<KnockbackScript>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        animator = GetComponentInChildren<Animator>();
        ratEnemyMovement = GetComponent<RatEnemyMovement>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
       if (knockedOut)
        {
            ratEnemyMovement.SetKnockedOut(true);
        }
    }

    public void ChangeHealth(int amount, Vector2 knockbackdirection, float hitDirectionForce, float additionalForce)
    {
        if (blobfishCombat == null || blobfishCombat.GetIsExpanding() == false)
        {
            StartCoroutine(enemyKnockbackScript.KnockbackAction(knockbackdirection, Vector2.up, hitDirectionForce, additionalForce));
            currentEnemyHealth += amount;
            CameraShakeManager.instance.CameraShake(impulseSource);
            damageFlash.GetDamageFlasher();
            hitParticles.Play();
        }

        if (currentEnemyHealth > maxEnemyHealth)
        {
            currentEnemyHealth = maxEnemyHealth;
        }
        else if (currentEnemyHealth <= 0)
        {
            Debug.Log("Enemy died");
            StartCoroutine(DeathSequence());
        }
        IEnumerator DeathSequence()
        {
            knockedOut = true;
            // Deactivate all
            animator.SetTrigger("RatDied");
            //while (spriteRenderer.color.) Make less opaque
            yield return new WaitForSeconds(3);
            animator.SetTrigger("PermaDied");
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
            knockedOut = false;
        }
    }
}
