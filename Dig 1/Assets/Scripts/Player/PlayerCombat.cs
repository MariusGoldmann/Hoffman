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
    [SerializeField] float boomerangAttackLengh;
    [SerializeField] float boomerangAttackForce;
    [SerializeField] float boomerangReturnAttackForce;

    Coroutine boomerangSpawnerCoroutine;

    // GameObjects
    [SerializeField] GameObject boomerangPrefab;

    // Script references
    [SerializeField] PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang"))
        {
            Debug.Log("Boomerang picked up");
            GameObject boomerang = collision.gameObject;

            Destroy(boomerang);

            boomerangAttackTimer = boomerangAttackCooldown;
        }
    }

    IEnumerator BoomerangSpawner()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + 2 * playerMovement.GetFacingDirection(), transform.position.y, transform.position.z);


        GameObject boomerang = Instantiate(boomerangPrefab, spawnPosition, Quaternion.identity);
        
        Rigidbody2D boomerangRB = boomerang.GetComponent<Rigidbody2D>();
        
        boomerangRB.linearVelocity = new Vector2(playerMovement.GetFacingDirection() * boomerangAttackForce, 0);
        
        yield return new WaitForSeconds(boomerangAttackLengh);
        
        while (boomerangRB != null && Vector2.Distance(boomerang.transform.position, transform.position) > 0.1f)
        {
            Vector2 direction = (transform.position - boomerang.transform.position).normalized;

            boomerangRB.linearVelocity = Vector2.Lerp(boomerangRB.linearVelocity, direction * boomerangReturnAttackForce, 8f * Time.deltaTime);

            yield return null;
        }

        boomerangSpawnerCoroutine = null;
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
        if (boomerangButton.isPressed && boomerangAttackTimer <= 0 && boomerangSpawnerCoroutine == null)
        {
            boomerangSpawnerCoroutine = StartCoroutine(BoomerangSpawner());
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
