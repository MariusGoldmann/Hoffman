using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] float boomerangLengh;
    [SerializeField] float boomerangForce;
    [SerializeField] float boomerangReturnForce;

    // Private variables
    Coroutine boomerangSpawnerCoroutine;

    // Script references
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PickUpScript pickUpScript;

    // Component references
    [SerializeField] GameObject boomerangPrefab;
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

        if (enemies != null)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<EnemyHealth>().ChangeHealth(-damage);
            }

            animator.SetTrigger(animation);
        }
    }

    IEnumerator BoomerangSpawner()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + 2 * playerMovement.GetFacingDirection(), transform.position.y, transform.position.z);


        GameObject boomerang = Instantiate(boomerangPrefab, spawnPosition, Quaternion.identity);
        
        Rigidbody2D boomerangRB = boomerang.GetComponent<Rigidbody2D>();

        float boomerangSpeed = boomerangForce;
        float boomerangReturnSpeed = boomerangReturnForce;
        int boomerangDirection = playerMovement.GetFacingDirection();
        
        while (boomerangSpeed > 1)
        {
            boomerangRB.linearVelocity = new Vector2(boomerangDirection *  boomerangSpeed, boomerangRB.linearVelocityY);
            boomerangSpeed = Mathf.Lerp(boomerangSpeed, 0, 1 * Time.deltaTime);

            Debug.Log(boomerangSpeed);
            yield return null;
        }

       // yield return new WaitForSeconds(boomerangLengh);
        
        while (boomerangRB != null && Vector2.Distance(boomerang.transform.position, transform.position) > 0.1f)
        {
            boomerang.transform.position = Vector2.MoveTowards(boomerang.transform.position , transform.position, boomerangReturnSpeed * Time.deltaTime);
            boomerangReturnSpeed += 50 * Time.deltaTime;


            yield return null;
        }

        boomerangSpawnerCoroutine = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang"))
        {
            Debug.Log("Boomerang picked up");
            GameObject boomerang = collision.gameObject;

            Destroy(boomerang);

            boomerangTimer = boomerangCooldown;
        }
    }

    void OnSlash(InputValue slashbutton)
    {
        if (slashbutton.isPressed && slashTimer <= 0 && pickUpScript.GetHasLeg())
        {
            MeleeAttack(slashDamage, "Slash");
            Debug.Log("Slash");
            slashTimer = slashCooldown;
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

    void OnDrawGizmos() // Attack radius debug
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
