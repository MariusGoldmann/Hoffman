using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour {
	private static readonly int Throwing = Animator.StringToHash("Throwing");
	[Header("Basic combat settings")]
	[SerializeField] private float attackRadius = 1.4f;
	[SerializeField] private Transform attackPoint;
	[SerializeField] private LayerMask enemyLayer;

	[Header("Knockback Settings")]
	[SerializeField] private float hitDirectionForce = 7.5f;
	[SerializeField] private float additionalForce = 7.5f;

	[Header("Slash settings")]
	[SerializeField] private int slashDamage = 1;
	[SerializeField] private float slashCooldown = 1f;
	[SerializeField] private float slashTimer;

	[Header("Kick settings")]
	[SerializeField] private int kickDamage = 2;
	[SerializeField] private float kickCooldown = 2f;
	[SerializeField] private float kickTimer;

	[Header("Boomerang settings")]
	[SerializeField] private int boomerangDamage = 5;
	[SerializeField] private float boomerangCooldown = 5f;
	[SerializeField] private float boomerangTimer;
	[SerializeField] private float boomerangForce;
	[SerializeField] private float boomerangReturnForce;
	[SerializeField] private float invulnerableTime;

	private bool earlyReceiving;
	private bool isInvulnerable;

	[SerializeField] private AnimationCurve boomerangAnimationCurve;

	[SerializeField] private Transform effectPoint; // drag in inspector
	// Private variables
	private Coroutine boomerangSpawnerCoroutine;

	// Script references
	private                  PlayerMovement     playerMovement;
	private                  PickUpScript       pickUpScript;
	private                  PauseManager       pauseManager;
	private                  HitStop            hitStop;
	[SerializeField] private DialogueController dialogueController;

	// Component references
	[SerializeField] private GameObject  boomerangPrefab; // drag in inspector
	[SerializeField] private GameObject  slashEffect;     // drag in inspector
	[SerializeField] private GameObject  kickEffect;      // drag in inspector
	private                  Rigidbody2D playerRb;
	private                  Animator    animator;

	public PlayerCombat(Transform attackPoint, LayerMask enemyLayer) {
		this.attackPoint = attackPoint;
		this.enemyLayer  = enemyLayer;
	}

	private void Start() {
		attackPoint = GetComponent<Transform>();
	}

	private void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
		pickUpScript   = GetComponent<PickUpScript>();
		hitStop        = GetComponent<HitStop>();
		playerRb        = GetComponent<Rigidbody2D>();
		pauseManager   = FindAnyObjectByType<PauseManager>();

		animator = GetComponentInChildren<Animator>();
	}

	private void Update() {
		HandleCooldowns();
	}

	private void MeleeAttack(int damage, string animationName, float knockbackMultiplier) {
		var enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
		animator.SetTrigger(animationName);

		if (enemies == null) return;
		foreach (var enemy in enemies) {
			Vector2 direction = (enemy.transform.position - transform.position).normalized;
			enemy.GetComponent<EnemyHealth>().ChangeHealth(-damage, direction,
			                                               hitDirectionForce * knockbackMultiplier,
			                                               additionalForce   * knockbackMultiplier,
			                                               enemy.transform.position);
			PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASHHIT);
			hitStop.Stop();
		}
	}

	private IEnumerator BoomerangSpawner() {
		var spawnPosition = new Vector3(transform.position.x + 2 * playerMovement.GetFacingDirection(),
		                                transform.position.y, transform.position.z);
		var boomerang = Instantiate(boomerangPrefab, spawnPosition, Quaternion.identity);

		var boomerangRb = boomerang.GetComponent<Rigidbody2D>();

		earlyReceiving = false;

		var         timer                = 0f;
		const float duration             = 0.5f;
		var         boomerangReturnSpeed = boomerangReturnForce;
		var         boomerangDirection   = playerMovement.GetFacingDirection(); //Where the player is facing


		while (timer < duration && !earlyReceiving) {
			isInvulnerable =  true;
			timer          += Time.deltaTime;
			var boomerangSpeed = boomerangForce * boomerangAnimationCurve.Evaluate(timer / duration);

			if (boomerangRb)
				boomerangRb.linearVelocity =
					new Vector2(boomerangDirection * boomerangSpeed, boomerangRb.linearVelocity.y);


			yield return null;
		}

		while (boomerang && Vector2.Distance(boomerang.transform.position, transform.position) > 0.1f ||
		       boomerang && earlyReceiving) {
			isInvulnerable       =  false;
			boomerangReturnSpeed += 50 * Time.deltaTime;
			boomerang.transform.position = Vector2.MoveTowards(
			                                                   boomerang.transform.position,
			                                                   transform.position,
			                                                   boomerangReturnSpeed * Time.deltaTime
			                                                   );

			earlyReceiving = true;
			yield return null;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("Boomerang") || isInvulnerable) return;
		Debug.Log("Boomerang picked up");
		var boomerang = collision.gameObject;
		earlyReceiving = false;

		boomerangSpawnerCoroutine = null;
		Destroy(boomerang);
	}

	private void OnSlash(InputValue slashButton) {
		if (!slashButton.isPressed || !(slashTimer <= 0) || !pickUpScript.GetHasLeg() || pauseManager.GetIsPaused() ||
		    dialogueController.GetIsInDialogue()) return;
		slashTimer = slashCooldown;
		MeleeAttack(slashDamage, "Slash", 1f);
		AttackEffects(slashEffect);
		PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.SLASH);
	}

	private void OnKick(InputValue kickButton) {
		if (!kickButton.isPressed || !(kickTimer <= 0) || !pickUpScript.GetHasLeg() || pauseManager.GetIsPaused() ||
		    dialogueController.GetIsInDialogue()) return;
		kickTimer = kickCooldown;
		MeleeAttack(kickDamage, "Kick", 2f);
		AttackEffects(kickEffect);
		PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.KICK);
	}

	private void OnBoomerang(InputValue boomerangButton) {
		if (!boomerangButton.isPressed      || !(boomerangTimer <= 0)     || boomerangSpawnerCoroutine != null ||
		    !pickUpScript.GetHasBoomerang() || pauseManager.GetIsPaused() ||
		    dialogueController.GetIsInDialogue()) return;
		boomerangTimer = boomerangCooldown;
		animator.SetTrigger(Throwing);
		boomerangSpawnerCoroutine = StartCoroutine(BoomerangSpawner());
		PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BOOMERANGTHROW);
	}

	private void AttackEffects(GameObject attackEffect) {
		var effect = Instantiate(attackEffect, effectPoint.position, Quaternion.identity);
		effect.transform.SetParent(transform);
		effect.transform.localScale = new Vector3(effect.transform.localScale.x * playerMovement.GetFacingDirection(),
		                                          effect.transform.localScale.y, effect.transform.localScale.z);
	}

	private void HandleCooldowns() {
		slashTimer -= Time.deltaTime;

		kickTimer -= Time.deltaTime;

		boomerangTimer -= Time.deltaTime;
	}

	public float GetSlashTimer() {
		return slashTimer;
	}

	public float GetKickTimer() {
		return kickTimer;
	}

	public float GetBoomerangTimer() {
		return boomerangTimer;
	}

	public int GetBoomerangDamage() {
		return boomerangDamage;
	}

	public bool GetEarlyReceiving(bool value) {
		return earlyReceiving = value;
	}
}