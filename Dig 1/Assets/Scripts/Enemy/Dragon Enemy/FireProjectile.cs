using UnityEngine;

public class FireProjectile : MonoBehaviour {
	[Header("General settings")]
	[SerializeField] int projectileDamage;
	[SerializeField] float explosionRadius;
	[SerializeField] float additionalForce   = 10f;
	[SerializeField] float hitDirectionForce = 10f;

	[Header("Particles")]
	[SerializeField] GameObject fireSplitter;
	[SerializeField] GameObject explosion;

	[Header("Layers")]
	[SerializeField] LayerMask groundLayer;
	[SerializeField] LayerMask playerLayer;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] LayerMask bomerangLayer;

	[SerializeField] ObjectPooling fireProjectilePool;

	void Awake() {
		gameObject.SetActive(true);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		var groundHit   = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, groundLayer);
		var playerHit   = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, playerLayer);
		var bomerangHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, bomerangLayer);

		if (groundHit != null) {
			gameObject.SetActive(false);
			Instantiate(fireSplitter, transform.position, Quaternion.identity);
			Instantiate(explosion,    transform.position, Quaternion.identity);
			gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
			gameObject.transform.localRotation                    = Quaternion.identity;
			fireProjectilePool.ReturnObject(gameObject);
		}

		if (playerHit != null) {
			gameObject.SetActive(false);
			Instantiate(fireSplitter, transform.position, Quaternion.identity);
			Instantiate(explosion,    transform.position, Quaternion.identity);
			PlayerHealth playerHealth = playerHit.GetComponent<PlayerHealth>();
			Vector2      hitDir       = (playerHit.transform.position - transform.position).normalized;
			playerHealth.ChangeHealth(-projectileDamage, hitDir, Vector2.up, hitDirectionForce, additionalForce,
			                          Vector3.zero, false);
			gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
			gameObject.transform.localRotation                    = Quaternion.identity;
			fireProjectilePool.ReturnObject(gameObject);
		}

		if (bomerangHit != null) {
			gameObject.SetActive(false);
			Instantiate(fireSplitter, transform.position, Quaternion.identity);
			Instantiate(explosion,    transform.position, Quaternion.identity);
			gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
			gameObject.transform.localRotation                    = Quaternion.identity;
			fireProjectilePool.ReturnObject(gameObject);
		}
	}
}