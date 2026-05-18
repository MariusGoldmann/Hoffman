using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CatsleOpener : MonoBehaviour {
	public static CatsleOpener instance;
	
	[SerializeField] public bool             gateOpened;
	
	[SerializeField] private float            interactRadius;
	[SerializeField] public bool             isWithinInteractRadius;

	[SerializeField] private GameObject keyMenuUi;
	
	[SerializeField] private CircleCollider2D interactCollider;
	[SerializeField] private BoxCollider2D    gateCollider;

	private void Awake() {
		if (instance != null) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		
		interactCollider = GetComponent<CircleCollider2D>();
		gateCollider = GetComponent<BoxCollider2D>();
	}
	private void Update() {
		interactCollider.radius = interactRadius;
		if (gateOpened) gateCollider.enabled = false;
		if (isWithinInteractRadius && Keyboard.current.eKey.wasPressedThisFrame) keyMenuUi.SetActive(true);
	}


	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) isWithinInteractRadius = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) isWithinInteractRadius = false;
	}
}
