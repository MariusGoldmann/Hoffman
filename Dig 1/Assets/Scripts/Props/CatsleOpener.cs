using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CatsleOpener : MonoBehaviour {
	public static CatsleOpener instance;
	
	[SerializeField] public bool             gateOpened;
	
	[SerializeField] private float            interactRadius;
	[SerializeField] public bool             isWithinInteractRadius;

	[SerializeField] private Transform        raycastTransform1;
	[SerializeField] private Transform        raycastTransform2;
	
	[SerializeField] private CircleCollider2D interactCollider;
	[SerializeField] private BoxCollider2D    gateCollider;

	private void Awake() {
		if (instance != null) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
		
		interactCollider = GetComponent<CircleCollider2D>();
		gateCollider = GetComponent<BoxCollider2D>();
	}
	private void Update() {
		interactCollider.radius = interactRadius;
		if (gateOpened) gateCollider.enabled = false;
		if (EnteredCastleSceneTrigger()) SceneManager.LoadScene("CastleScene");
	}


	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) isWithinInteractRadius = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) isWithinInteractRadius = false;
	}

	private bool EnteredCastleSceneTrigger() {
		var hit = Physics2D.Linecast(raycastTransform1.position, raycastTransform2.position, 
		                             LayerMask.GetMask("Player"));
		return hit;
	}

	public void OnDrawGizmos() {
		Color c = Color.red;
		Gizmos.DrawLine(raycastTransform1.position, raycastTransform2.position);
	}
}
