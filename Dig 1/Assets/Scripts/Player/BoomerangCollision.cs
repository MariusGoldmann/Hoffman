using UnityEngine;

public class BoomerangColission : MonoBehaviour
{
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalForce = 5f;
    // Script references
    PlayerCombat playerCombat;

    void Awake()
    {
        playerCombat = FindAnyObjectByType<PlayerCombat>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth;

            GameObject enemy = collision.gameObject;
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            enemyHealth = enemy.gameObject.GetComponentInChildren<EnemyHealth>();
            
            Debug.Log("Enemy hit");
            enemyHealth.ChangeHealth(-playerCombat.GetBoomerangDamage(), direction, hitDirectionForce, additionalForce, collision.GetContact(0).point);
            playerCombat.GetEarlyReceiving(true);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground hit");
            playerCombat.GetEarlyReceiving(true);
        }
    }
}
