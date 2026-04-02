using Cinemachine;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;

    [SerializeField] float timeDead=3f;

    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalDirectionalForce = 5f;

    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] DamageFlash damageFlash;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] BlobfishCombat blobfishCombat;

    CinemachineImpulseSource impulseSource;


    SpriteRenderer spriteRenderer;

    Animator animator;

    void Awake()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        knockbackScript = GetComponent<KnockbackScript>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ChangeHealth(int amount, Vector2 knockbackdirection)
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        if (blobfishCombat == null || blobfishCombat.GetIsExpanding() == false)
        {
            StartCoroutine(knockbackScript.KnockbackAction(knockbackdirection, Vector2.up, hitDirectionForce, additionalDirectionalForce));
            currentEnemyHealth += amount;
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
            // Deactivate all
            animator.SetTrigger("RatDied");
            //while (spriteRenderer.color.) Make less opaque
            yield return null;
            Destroy(gameObject);
        }

        damageFlash.GetDamageFlasher();
    }
}
