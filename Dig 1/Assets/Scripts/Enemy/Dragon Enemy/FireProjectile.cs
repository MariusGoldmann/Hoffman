using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] int projectileDamage;
    [SerializeField] float explosionRadius;
    [SerializeField] float additionalForce = 10f;
    [SerializeField] float hitDirectionForce = 10f;

    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] ObjectPooling fireProjectilePool;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D groundHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, groundLayer);
        Collider2D playerHit  = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, playerLayer);
        Collider2D enemyHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, enemyLayer);

        if (groundHit != null)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            fireProjectilePool.ReturnObject(gameObject);
        }
        if (playerHit != null)
        {
            PlayerHealth playerHealth = playerHit.GetComponent<PlayerHealth>();
            Vector2 hitDir = (playerHit.transform.position - transform.position).normalized;
            playerHealth.ChangeHealth(-projectileDamage, hitDir, Vector2.up, hitDirectionForce, additionalForce, Vector3.zero, false);
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            fireProjectilePool.ReturnObject(gameObject);
        }
        if (enemyHit != null)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            EnemyHealth enemyHealth = enemyHit.GetComponent<EnemyHealth>();
            Vector2 hitDir = (enemyHit.transform.position - transform.position).normalized;
            enemyHealth.ChangeHealth(-projectileDamage, hitDir, hitDirectionForce, additionalForce, Vector3.zero);
            fireProjectilePool.ReturnObject(gameObject);
        }
    }
}
