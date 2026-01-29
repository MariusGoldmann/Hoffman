using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
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


    void OnMelee(InputValue meleebutton)
    {
        if (meleebutton.isPressed && meleeAttackTimer <= 0)
        {
            Debug.Log("Melee");
            meleeAttackTimer = meleeAttackCooldown;
        }
    }

    void OnKick(InputValue kickButton)
    {
        if (kickButton.isPressed && kickAttackTimer <= 0)
        {
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
}
