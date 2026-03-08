using UnityEngine;

public class BoomerangColission : MonoBehaviour
{
    // Script references
    PlayerCombat playerCombat;
    EnemyHealth enemyHealth;

    void Awake()
    {
        playerCombat = FindAnyObjectByType<PlayerCombat>();
        enemyHealth = FindAnyObjectByType<EnemyHealth>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            enemyHealth.ChangeHealth(-playerCombat.GetBoomerangDamage());
            playerCombat.GetEarlyReceiving(true);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground hit");
            playerCombat.GetEarlyReceiving(true);
        }

        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
