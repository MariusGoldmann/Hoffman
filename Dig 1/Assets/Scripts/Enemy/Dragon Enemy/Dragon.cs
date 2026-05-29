using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour {
	private static readonly int FireCharge = Animator.StringToHash("FireCharge");
	private static readonly int Death      = Animator.StringToHash("Death");
	private static readonly int Knockback  = Animator.StringToHash("Knockback");
	private static readonly int PermaDeath = Animator.StringToHash("PermaDeath");
	private static readonly int Idle       = Animator.StringToHash("Idle");
	private static readonly int Patrolling = Animator.StringToHash("Patrolling");
	[Header("General settings")]
	[SerializeField] private float idleMoveSpeed;
	[SerializeField] private float recoilForce;
	[SerializeField] private float detectionRadius;
	[SerializeField] private float aggroTime;
	[SerializeField] private float aggroTimer;

	[Header("Attack settings")]
	[SerializeField] private int attackCooldown;
	[SerializeField] private int projectileSpeed;
	[SerializeField] private int anticipationTime;
	[SerializeField] private Transform attackPoint;

	[Header("Death settings")]
	[SerializeField] private float deathTime;
	[SerializeField] private bool isDead = false;
	[SerializeField] GameObject hpParticlePrefab;

	[Header(("Animation bool"))]
	[SerializeField] private bool isPatrolling;

	[Header("Raycast/Collider settings")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform  wallCheck;
	[SerializeField] private float      groundCheckDistance;
	[SerializeField] private float      wallCheckDistance;

	//Private variables
	private float     cooldownTimer;
	private int       facingDirection = 1;
	[SerializeField] private bool      isKnockback = false;
	private LayerMask groundLayer;

	//Script/Component references
	private KnockbackScript dragonKnockBackScript;
	private EnemyHealth     enemyHealth;
	private Rigidbody2D     dragonRb;
	private Animator        dragonAnimator;
    [SerializeField] private GameObject		player;
    [SerializeField] private ObjectPooling fireProjectilePool;


    private Coroutine rangedAttackCoroutine;
	private Coroutine deathCoroutine;

	private void Awake() {
		groundLayer           = LayerMask.GetMask("Ground");
		dragonKnockBackScript = GetComponent<KnockbackScript>();
		enemyHealth           = GetComponent<EnemyHealth>();
		dragonRb              = GetComponent<Rigidbody2D>();
		dragonAnimator = GetComponentInChildren<Animator>();
	}

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		fireProjectilePool= GameObject.FindGameObjectWithTag("FirePool").GetComponent<ObjectPooling>();
    }

	private void Update() {
		KnockbackUpdater();
	}

	private void FixedUpdate() {
		Patrol();
		Flip();
		HandleCooldown();
		DeathSequence();
	}

	private void Patrol() {
		if (!IsPlayerDetected() && !isDead && aggroTimer < 0) {
			dragonRb.linearVelocityX = idleMoveSpeed * facingDirection;
			dragonAnimator.SetBool(Idle,       false);
			dragonAnimator.SetBool(Patrolling, true);
			rangedAttackCoroutine    = null;
		} else {
			dragonRb.linearVelocity = new Vector2(dragonRb.linearVelocity.x, dragonRb.linearVelocity.y);
			dragonAnimator.SetBool(Idle,       true);
			dragonAnimator.SetBool(Patrolling, false);
			if (cooldownTimer <= 0 && !isDead && PlayerTarget()) {
				rangedAttackCoroutine = StartCoroutine(RangedAttack());
			}
		}
	}

	private IEnumerator RangedAttack() {
		dragonAnimator.SetTrigger(FireCharge);
		cooldownTimer = attackCooldown;
		yield return new WaitForSeconds(anticipationTime);

		PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.FIREBALL);
		dragonRb.linearVelocity = new Vector2((recoilForce) * facingDirection * -1, recoilForce);
		Vector2 fireDirection = (PlayerTarget().position - attackPoint.position).normalized;
		
		var   angle         = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
		var  rotation     = Quaternion.Euler(0, 0, angle);
		var  projectile   = fireProjectilePool.GetObject(attackPoint.position, rotation);
		var projectileRb = projectile.GetComponent<Rigidbody2D>();
		projectileRb.linearVelocity = fireDirection * projectileSpeed;

		rangedAttackCoroutine = null;
	}

	private void DeathSequence() {
		if (!(enemyHealth.GetHealth() <= 0)) return;
		isDead                  = true;
		dragonRb.linearVelocity = Vector2.zero;
		if (deathCoroutine != null) {
			return;
		}

		deathCoroutine = StartCoroutine(DeathCoroutine());
	}

	private void KnockbackUpdater() {
		isKnockback = dragonKnockBackScript.GetIsKnockback();
		dragonAnimator.SetBool(Knockback, isKnockback);
		if (isKnockback) {
			cooldownTimer = attackCooldown;
		}
	}

	private IEnumerator DeathCoroutine() {
		dragonAnimator.SetTrigger(Death);
		Debug.Log("Dragon Dies");
		yield return new WaitForSeconds(3);
		dragonAnimator.SetTrigger(PermaDeath);
		yield return new WaitForSeconds(3); for (int i = 0; i <= Random.Range(0, 3); i++)
        {
            Instantiate(hpParticlePrefab, new Vector2(transform.position.x + i, transform.position.y), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
	}

	private void Flip() {
		if (IsAtEdge() && !isDead || IsAtWall() && !isDead) {
			facingDirection = facingDirection * -1;
		}

		if (PlayerTarget() && !isDead) {
			facingDirection = (int)Mathf.Sign(PlayerTarget().position.x - transform.position.x);
		}

		transform.localScale = new Vector2(facingDirection, 1);
	}

	private void HandleCooldown() {
		cooldownTimer -= Time.deltaTime;
		if (IsPlayerDetected()) {
			aggroTimer = aggroTime;
		} else {
			aggroTimer -=  Time.deltaTime;
		}
	}

	private bool IsAtEdge() {
		return !Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
	}

	private bool IsAtWall() {
		return Physics2D.Raycast(wallCheck.position, Vector2.up, wallCheckDistance, groundLayer);
	}

	private Transform PlayerTarget() {
		if ((Vector2.Distance(transform.position, player.transform.position) < detectionRadius) || aggroTimer > 0f) {
			var hit = Physics2D.Linecast(transform.position, player.transform.position,
			                             ~LayerMask.GetMask("Enemy", "FireProjectile"));
			if (hit.collider.gameObject.CompareTag("Player")) {
				return hit.transform;
			}
		}
		return null;
	}

	private bool IsPlayerDetected() {
		if (!(Vector2.Distance(transform.position, player.transform.position) < detectionRadius)) return false;
		var hit = Physics2D.Linecast(transform.position, player.transform.position,
		                             ~LayerMask.GetMask("Enemy", "FireProjectile"));

		return hit.collider.gameObject.CompareTag("Player");
	}
}