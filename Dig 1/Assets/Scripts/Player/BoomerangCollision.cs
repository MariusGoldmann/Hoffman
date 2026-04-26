using UnityEngine;

public class BoomerangColission : MonoBehaviour
{
    [SerializeField] private float hitDirectionForce = 10f;
    [SerializeField] private float additionalForce   = 5f;
    // Script references
    private PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = FindAnyObjectByType<PlayerCombat>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
	        var  enemy       = collision.gameObject;
            Vector2     direction   = (enemy.transform.position - transform.position).normalized;
            var enemyHealth = enemy.gameObject.GetComponentInChildren<EnemyHealth>();
            
            Debug.Log("Enemy hit");
            enemyHealth.ChangeHealth(-playerCombat.GetBoomerangDamage(), direction, hitDirectionForce, additionalForce, collision.GetContact(0).point);
            playerCombat.GetEarlyReceiving(true);
        }

        if (!collision.gameObject.CompareTag("Ground")) return;
        Debug.Log("Ground hit");
        playerCombat.GetEarlyReceiving(true);
    }
}
