using UnityEngine;

public class FireProjectile : MonoBehaviour {
	[Header("General settings")]
	[SerializeField] private int projectileDamage;
	[SerializeField] private float explosionRadius;
	[SerializeField] private float additionalForce   = 10f;
	[SerializeField] private float hitDirectionForce = 10f;

	[Header("Particles")]
	[SerializeField] private GameObject fireSplitter;
	[SerializeField] private GameObject explosion;

	[Header("Layers")]
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private LayerMask enemyLayer;
	[SerializeField] private LayerMask boomerangLayer;

	private ObjectPooling fireProjectilePool;

	void Awake() {
		gameObject.SetActive(true);
		fireProjectilePool= GameObject.FindGameObjectWithTag("FirePool").GetComponent<ObjectPooling>();
    }

	void OnTriggerEnter2D(Collider2D collision) {
		var groundHit   = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, groundLayer);
		var playerHit   = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, playerLayer);
		var boomerangHit = Physics2D.OverlapCircle(gameObject.transform.position, explosionRadius, boomerangLayer);

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

		if (boomerangHit == null) return;
		gameObject.SetActive(false);
		Instantiate(fireSplitter, transform.position, Quaternion.identity);
		Instantiate(explosion,    transform.position, Quaternion.identity);
		gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
		gameObject.transform.localRotation                    = Quaternion.identity;
		fireProjectilePool.ReturnObject(gameObject);
	}
}