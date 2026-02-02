using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Basic combat settings")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemyLayer;

    [Header("Slash settings")]
    [SerializeField] int slashDamage = 1;
    [SerializeField] float slashAttackCooldown = 1f;
    [SerializeField] float slashAttackTimer;

    [Header("Kick settings")]
    [SerializeField] int kickDamage = 2;
    [SerializeField] float kickAttackCooldown = 2f;
    [SerializeField] float kickAttackTimer;

    [Header("Boomerang settings")]
    [SerializeField] int boomerangDamage = 5;
    [SerializeField] float boomerangAttackCooldown = 5f;
    [SerializeField] float boomerangAttackTimer;

    [SerializeField] GameObject boomerangPrefab;

    void Start()
    {
        slashAttackTimer = slashAttackCooldown;
        kickAttackTimer = kickAttackCooldown;
        boomerangAttackTimer = boomerangAttackCooldown;
    }

    void Update()
    {
        HandleCooldowns();
    }

    void SlashAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        if (enemies != null)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<EnemyHealth>().ChangeHealth(-slashDamage);
            }
           
        }
    }

    void KickAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        if (enemies != null)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<EnemyHealth>().ChangeHealth(-kickDamage);
            }
        }
    }

    void BoomerangAttack()
    {
        Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
    }

    void OnSlash(InputValue slashbutton)
    {
        if (slashbutton.isPressed && slashAttackTimer <= 0)
        {
            SlashAttack();
            Debug.Log("Slash");
            slashAttackTimer = slashAttackCooldown;
        }
    }

    void OnKick(InputValue kickButton)
    {
        if (kickButton.isPressed && kickAttackTimer <= 0)
        {
            KickAttack();
            Debug.Log("Kick");
            kickAttackTimer = kickAttackCooldown;
        }
    }

    void OnBoomerang(InputValue boomerangButton)
    {
        if (boomerangButton.isPressed && boomerangAttackTimer <= 0)
        {
            BoomerangAttack();
            Debug.Log("Boomerang");
            boomerangAttackTimer = boomerangAttackCooldown;
        }
    }
    void HandleCooldowns()
    {
        if (slashAttackTimer > 0)
        {
            slashAttackTimer -= Time.deltaTime;
        }

        if (kickAttackTimer > 0)
        {
            kickAttackTimer -= Time.deltaTime;
        }

        if (boomerangAttackTimer > 0)
        {
            boomerangAttackTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
