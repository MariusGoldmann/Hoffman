using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerMovement;

public class PlayerCombat : MonoBehaviour
{
    [Header("Basic combat settings")]
    [SerializeField] float attackRadius = 1.4f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    [Header("Knockback Settings")]
    [SerializeField] float hitDirectionForce = 7.5f;
    [SerializeField] float additionalForce = 7.5f;

    [Header("Slash settings")]
    [SerializeField] int slashDamage = 1;
    [SerializeField] float slashCooldown = 1f;
    [SerializeField] float slashTimer;

    [Header("Kick settings")]
    [SerializeField] int kickDamage = 2;
    [SerializeField] float kickCooldown = 2f;
    [SerializeField] float kickTimer;

    [Header("Boomerang settings")]
    [SerializeField] int boomerangDamage = 5;
    [SerializeField] float boomerangCooldown = 5f;
    [SerializeField] float boomerangTimer;
    [SerializeField] float boomerangForce;
    [SerializeField] float boomerangReturnForce;

    [SerializeField] bool earlyReceiving;

    [SerializeField] AnimationCurve boomerangAnimationCurve;

    [SerializeField] Transform effectPoint; // drag in inspector
    // Private variables
    Coroutine boomerangSpawnerCoroutine;

    // Script references
    PlayerMovement playerMovement;
    PickUpScript pickUpScript;
    PauseManager pauseManager;
    [SerializeField] DialogueController dialogueController;

    // Component references
    [SerializeField] GameObject boomerangPrefab; // drag in inspector
    [SerializeField] GameObject slashEffect; // drag in inspector
    [SerializeField] GameObject kickEffect; // drag in inspector
    Animator animator;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        pickUpScript = GetComponent<PickUpScript>();
        pauseManager = FindAnyObjectByType<PauseManager>();

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleCooldowns();
    }

    void MeleeAttack(int damage, string animation, float knockbackMultiplier)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
            
        animator.SetTrigger(animation);

        if (enemies != null)
        {
            foreach (Collider2D enemy in enemies)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponentInChildren<EnemyHealth>().ChangeHealth(-damage, direction, hitDirectionForce*knockbackMultiplier, additionalForce*knockbackMultiplier, enemy.transform.position);
                PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASHHIT, 1f);
            }
        }
    }
    IEnumerator BoomerangSpawner()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + 2 * playerMovement.GetFacingDirection(), transform.position.y, transform.position.z);
        GameObject boomerang = Instantiate(boomerangPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D boomerangRB = boomerang.GetComponent<Rigidbody2D>();

        earlyReceiving = false;

        float timer = 0;
        float duration = 0.5f;
        float boomerangSpeed;
        float boomerangReturnSpeed = boomerangReturnForce;
        int boomerangDirection = playerMovement.GetFacingDirection(); //Where the player is facing

        while (timer < duration && !earlyReceiving)
        {
            timer += Time.deltaTime;
            boomerangSpeed = boomerangForce * boomerangAnimationCurve.Evaluate(timer / duration);

            if (boomerangRB!=null) boomerangRB.linearVelocity = new Vector2(boomerangDirection * boomerangSpeed, boomerangRB.linearVelocity.y);

            yield return null;
        }

        while (boomerang != null && Vector2.Distance(boomerang.transform.position, transform.position) > 0.1f || boomerang != null && earlyReceiving)
        {
            boomerangReturnSpeed += 50 * Time.deltaTime;
            boomerang.transform.position = Vector2.MoveTowards(boomerang.transform.position, transform.position, boomerangReturnSpeed * Time.deltaTime);

            earlyReceiving = true;
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang"))
        {
            Debug.Log("Boomerang picked up");
            GameObject boomerang = collision.gameObject;
            earlyReceiving = false;

            boomerangSpawnerCoroutine = null;
            Destroy(boomerang);
        }
    }

    void OnSlash(InputValue slashbutton)
    {
        if (slashbutton.isPressed && slashTimer <= 0 && pickUpScript.GetHasLeg() && !pauseManager.GetIsPaused() && !dialogueController.GetIsInDialogue())
        {
            slashTimer = slashCooldown;
            MeleeAttack(slashDamage, "Slash", 1f);
            AttackEffects(slashEffect);
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASH, 1f);
        }
    }

    void OnKick(InputValue kickButton)
    {
        if (kickButton.isPressed && kickTimer <= 0 && pickUpScript.GetHasLeg() && !pauseManager.GetIsPaused() && !dialogueController.GetIsInDialogue())
        {
            kickTimer = kickCooldown;
            MeleeAttack(kickDamage, "Kick", 2f);
            AttackEffects(kickEffect);
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.KICK, 1f);
        }
    }

    void OnBoomerang(InputValue boomerangButton)
    {
        if (boomerangButton.isPressed && boomerangTimer <= 0 && boomerangSpawnerCoroutine == null && pickUpScript.GetHasBoomerang() && !pauseManager.GetIsPaused() && !dialogueController.GetIsInDialogue())
        {
            boomerangTimer = boomerangCooldown;
            animator.SetTrigger("Throwing");
            boomerangSpawnerCoroutine = StartCoroutine(BoomerangSpawner());
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BOOMERANGTHROW, 1f);
        }
    }

    void AttackEffects(GameObject attackEffect)
    {
        GameObject effect = Instantiate(attackEffect, effectPoint.position, Quaternion.identity);
        effect.transform.SetParent(transform);
        effect.transform.localScale = new Vector3(effect.transform.localScale.x * playerMovement.GetFacingDirection(), effect.transform.localScale.y, effect.transform.localScale.z);
    }
    void HandleCooldowns()
    {
        slashTimer -= Time.deltaTime;

        kickTimer -= Time.deltaTime;

        boomerangTimer -= Time.deltaTime;
    }

    public float GetSlashTimer()
    {
        return slashTimer;
    }

    public float GetKickTimer()
    {
        return kickTimer;
    }

    public float GetBoomerangTimer()
    {
        return boomerangTimer;
    }

    public int GetBoomerangDamage()
    {
        return boomerangDamage;
    }

    public bool GetEarlyReceiving(bool value)
    {
        return earlyReceiving = value;
    }
}


