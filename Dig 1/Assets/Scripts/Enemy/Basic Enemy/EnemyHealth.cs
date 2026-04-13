using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;



    [SerializeField] bool knockedOut = false;

    [SerializeField] DamageFlash damageFlash;
    [SerializeField] KnockbackScript enemyKnockbackScript;
    [SerializeField] BlobfishCombat blobfishCombat;
    [SerializeField] RatEnemyMovement ratEnemyMovement;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem hpParticleSpawnParticles;
    [SerializeField] GameObject hpParticlePrefab;

    [SerializeField] CinemachineImpulseSource impulseSource;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake()
    {
        damageFlash = GetComponentInChildren<DamageFlash>();
        enemyKnockbackScript = GetComponent<KnockbackScript>();
        animator = GetComponentInChildren<Animator>();
        ratEnemyMovement = GetComponent<RatEnemyMovement>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 
    }

    void Update()
    {
       if (knockedOut)
        {
            ratEnemyMovement.SetKnockedOut(true);
        }
    }

    public void ChangeHealth(int amount, Vector2 knockbackdirection, float hitDirectionForce, float additionalForce, Vector3 collisionPoint)
    {
        if (blobfishCombat == null || !blobfishCombat.GetIsExpanding())
        {
            if (currentEnemyHealth>0 && blobfishCombat==null) StartCoroutine(enemyKnockbackScript.KnockbackAction(knockbackdirection, Vector2.up, hitDirectionForce, additionalForce));
            currentEnemyHealth += amount;
            CameraShakeManager.instance.CameraShake(impulseSource);
            damageFlash.GetDamageFlasher();
            hitParticles.transform.position= collisionPoint;
            hitParticles.Play();
            ratEnemyMovement.TurnAround(knockbackdirection.x);
        }
        if (currentEnemyHealth > maxEnemyHealth)
        {
            currentEnemyHealth = maxEnemyHealth;
        }
        else if (currentEnemyHealth <= 0 && knockedOut==false)
        {
            Debug.Log("Enemy died");
            StartCoroutine(DeathSequence());
        }
    }
    IEnumerator DeathSequence()
        {
            if (blobfishCombat == null)
            {
                float timeLeft = 13f;
                bool hasDroppedHP = false;
                knockedOut = true;
                if (animator != null) animator.SetTrigger("RatDied");
                while (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    spriteRenderer.color = new Color(1f, 1f, 1f, timeLeft / 13);
                    if (timeLeft < 10 && !hasDroppedHP)
                    {
                        for (int i = 0; i < Random.Range(1, 3); i++)
                        {
                            hpParticleSpawnParticles.Play();
                            Instantiate(hpParticlePrefab, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                        }
                        if (animator != null) animator.SetTrigger("PermaDied");
                        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                        GetComponent<Collider2D>().enabled = false;
                        hasDroppedHP = true;
                    }
                    yield return null;
                }
                Destroy(gameObject);
            }
            else
            {
                for (int i = 0; i < Random.Range(1, 3); i++)
                {
                    Instantiate(hpParticlePrefab, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
                    hitParticles.Play(); 
                    CameraShakeManager.instance.CameraShake(impulseSource);
                    yield return new WaitForSeconds(0.1f);
                }
                Destroy(gameObject);
            }
    }

    public bool GetKnockedOut()
    {
        return knockedOut;
    }
}
