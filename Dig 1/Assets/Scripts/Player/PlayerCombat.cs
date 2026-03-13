using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerMovement;

public class PlayerCombat : MonoBehaviour
{
    [Header("Basic combat settings")]
    [SerializeField] float attackRadius;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    [Header("Slash settings")]
    [SerializeField] int slashDamage = 1;
    [SerializeField] int slashCooldown = 1;
    [SerializeField] float slashTimer;

    [Header("Kick settings")]
    [SerializeField] int kickDamage = 2;
    [SerializeField] int kickCooldown = 2;
    [SerializeField] float kickTimer;

    [Header("Boomerang settings")]
    [SerializeField] int boomerangDamage = 5;
    [SerializeField] int boomerangCooldown = 5;
    [SerializeField] float boomerangTimer;
    [SerializeField] float boomerangForce;
    [SerializeField] float boomerangReturnForce;

    [SerializeField] bool earlyReceiving;

    [SerializeField] AnimationCurve boomerangAnimationCurve;

    // Private variables
    Coroutine boomerangSpawnerCoroutine;

    // Script references
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PickUpScript pickUpScript;

    // Component references
    [SerializeField] GameObject boomerangPrefab; // drag in inspector
    Animator animator;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        pickUpScript = GetComponent<PickUpScript>();

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleCooldowns();
    }

    void MeleeAttack(int damage, string animation)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        animator.SetTrigger(animation);

        if (enemies != null)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<EnemyHealth>().ChangeHealth(-damage);
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

            boomerangRB.linearVelocity = new Vector2(boomerangDirection * boomerangSpeed, boomerangRB.linearVelocity.y);

            yield return null;
        }
        
        while (boomerang != null && Vector2.Distance(boomerang.transform.position, transform.position) > 0.1f || earlyReceiving)
        {
            boomerangReturnSpeed += 50 * Time.deltaTime;
            boomerang.transform.position = Vector2.MoveTowards(boomerang.transform.position , transform.position, boomerangReturnSpeed * Time.deltaTime);

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

            boomerangTimer = boomerangCooldown;
        }
    }

    void OnSlash(InputValue slashbutton)
    {
        if (slashbutton.isPressed && slashTimer <= 0 && pickUpScript.GetHasLeg())
        {
            slashTimer = slashCooldown;
            MeleeAttack(slashDamage, "Slash");
            Debug.Log("Slash");
        }
    }

    void OnKick(InputValue kickButton)
    {
        if (kickButton.isPressed && kickTimer <= 0 && pickUpScript.GetHasLeg())
        {
            Debug.Log("Kick");
            MeleeAttack(kickDamage, "Kick");
            kickTimer = kickCooldown;
        }
    }

    void OnBoomerang(InputValue boomerangButton)
    {
        if (boomerangButton.isPressed && boomerangTimer <= 0 && boomerangSpawnerCoroutine == null && pickUpScript.GetHasBoomerang())
        {
            Debug.Log("Throw");
            animator.SetTrigger("Throwing");
            boomerangSpawnerCoroutine = StartCoroutine(BoomerangSpawner());
        }
    }
    void HandleCooldowns()
    {
        slashTimer -= Time.deltaTime;

        kickTimer -= Time.deltaTime;

        boomerangTimer -= Time.deltaTime;
    }

    public int GetBoomerangDamage()
    {
        return boomerangDamage;
    }

    public bool GetEarlyReceiving(bool value)
    {
        return earlyReceiving = value;
    }

    void OnDrawGizmos() // Attack radius debug
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
