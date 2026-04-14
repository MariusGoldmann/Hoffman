using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] int damage;
    [SerializeField] float explosionRadius;
    [SerializeField] float additionalForce = 10f;
    [SerializeField] float hitDirectionForce = 10f;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D groundHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, groundLayer);
        Collider2D playerHit  = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, playerLayer);
        Collider2D enemyHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, enemyLayer);

        if (groundHit != null)
        {
            Destroy(gameObject);
        }
        if (playerHit != null)
        {
            PlayerHealth playerHealth = playerHit.GetComponent<PlayerHealth>();
            Vector2 hitDir = (playerHit.transform.position - transform.position).normalized;
            playerHealth.ChangeHealth(-damage, hitDir, Vector2.up, hitDirectionForce, additionalForce, Vector3.zero, false);
            Destroy(gameObject);
        }
        if (enemyHit != null)
        {
            EnemyHealth enemyHealth = enemyHit.GetComponent<EnemyHealth>();
            Vector2 hitDir = (enemyHit.transform.position - transform.position).normalized;
            enemyHealth.ChangeHealth(-damage, hitDir, hitDirectionForce, additionalForce, Vector3.zero);
            Destroy(gameObject);
        }
    }
}
