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

    [Header("Melee settings")]
    [SerializeField] int meleeDamage = 1;
    [SerializeField] float meleeAttackCooldown = 1f;
    [SerializeField] float meleeAttackTimer;

    [Header("Kick settings")]
    [SerializeField] int kickDamage = 2;
    [SerializeField] float kickAttackCooldown = 2f;
    [SerializeField] float kickAttackTimer;

    [Header("Bomerang settings")]
    [SerializeField] int bomerangDamage = 5;
    [SerializeField] float bomerangAttackCooldown = 5f;
    [SerializeField] float bomerangAttackTimer;


    void Update()
    {
        HandleCooldowns();
    }

    void MeleeAttack()
    {
        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);

        if (enemy != null)
        {
            enemy.gameObject.GetComponent<EnemyHealth>().ChangeHealth(-meleeDamage);
        }
    }

    void KickAttack()
    {
        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);

        if (enemy != null)
        {
            enemy.gameObject.GetComponent<EnemyHealth>().ChangeHealth(-kickDamage);
        }
    }

    void OnMelee(InputValue meleebutton)
    {
        if (meleebutton.isPressed && meleeAttackTimer <= 0)
        {
            MeleeAttack();
            Debug.Log("Melee");
            meleeAttackTimer = meleeAttackCooldown;
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

    void OnBomerang(InputValue bomerangButton)
    {
        if (bomerangButton.isPressed && bomerangAttackTimer <= 0)
        {
            Debug.Log("Bomerang");
            bomerangAttackTimer = bomerangAttackCooldown;
        }
    }

    void HandleCooldowns()
    {
        if (meleeAttackTimer > 0)
        {
            meleeAttackTimer -= Time.deltaTime;
        }

        if (kickAttackTimer > 0)
        {
            kickAttackTimer -= Time.deltaTime;
        }

        if (bomerangAttackTimer > 0)
        {
            bomerangAttackTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
