using UnityEngine;

public class BoomerangColission : MonoBehaviour
{
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
            enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
            
            Debug.Log("Enemy hit");
            enemyHealth.ChangeHealth(-playerCombat.GetBoomerangDamage(), direction);
            playerCombat.GetEarlyReceiving(true);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground hit");
            playerCombat.GetEarlyReceiving(true);
        }
    }
}
