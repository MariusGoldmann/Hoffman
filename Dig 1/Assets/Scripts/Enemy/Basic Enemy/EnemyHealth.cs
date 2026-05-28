using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;



    [SerializeField] bool knockedOut = false;

    [Header("References")]
    [SerializeField] KnockbackScript enemyKnockbackScript;
    [SerializeField] BlobfishCombat blobfishCombat;
    [SerializeField] RatEnemyMovement ratEnemyMovement;
    [SerializeField] ElephantMovement elephantMovement;


    DamageFlash damageFlash;
    ParticleSystem hitParticles;
    CinemachineImpulseSource impulseSource;
    HitStop hitStop;

    void Awake()
    {
        damageFlash = GetComponentInChildren<DamageFlash>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        hitStop = GetComponent<HitStop>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 
    }

    void Update()
    {
       if (ratEnemyMovement!=null && knockedOut) ratEnemyMovement.SetKnockedOut(true);
    }

    public void ChangeHealth(int amount, Vector2 knockbackdirection, float hitDirectionForce, float additionalForce, Vector3 collisionPoint)
    {
        if (blobfishCombat == null || !blobfishCombat.GetIsExpanding())
        {
            if (currentEnemyHealth>0 && blobfishCombat==null) StartCoroutine(enemyKnockbackScript.KnockbackAction(knockbackdirection, Vector2.up, hitDirectionForce, additionalForce));
            currentEnemyHealth += amount;
            hitStop.Stop();
            CameraShakeManager.instance.CameraShake(impulseSource);
            damageFlash.GetDamageFlasher();
            hitParticles.transform.position=collisionPoint;
            hitParticles.Play();
            if (ratEnemyMovement!=null && currentEnemyHealth > 0) ratEnemyMovement.TurnAround(knockbackdirection.x);
        }
        if (currentEnemyHealth > maxEnemyHealth)
        {
            currentEnemyHealth = maxEnemyHealth;
        }
        if (currentEnemyHealth <= 0 && knockedOut==false)
        {
            Debug.Log("Enemy died");
            DeathSequence();
        }
    }

    void DeathSequence()
    {
        BossSpawnStart bossSpawnStart = FindFirstObjectByType<BossSpawnStart>();
        if (bossSpawnStart != null) bossSpawnStart.enemyCountBoss -= 1;
        if (blobfishCombat != null) StartCoroutine(blobfishCombat.DeathSequence());
        if (ratEnemyMovement != null) StartCoroutine(ratEnemyMovement.DeathSequence());
        if (elephantMovement != null) StartCoroutine(elephantMovement.DeathSequence());
    }

    public bool GetKnockedOut()
    {
        return knockedOut;
    }

    public float GetHealth()
    {
        return currentEnemyHealth;
    }
}
