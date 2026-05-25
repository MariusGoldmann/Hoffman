using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteracableBear : MonoBehaviour {
	private                  Animator  bearAnimator;
	[SerializeField] private LayerMask clickable;

	public bool animationPlaying;

	private void Awake() {
		bearAnimator = GetComponentInChildren<Animator>();
	}

	private void Update() {
		if (Mouse.current.leftButton.wasPressedThisFrame) {
			Click();
		}
	}

	private void Click() {
		if (!Camera.main) return;
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			
		var hit = Physics2D.Raycast(mousePosition, Camera.main.transform.forward,1000, clickable );
		if (hit.collider.CompareTag("Bear")) {
			bearAnimator.SetTrigger("Animation 1");
			Debug.Log("Animation 1");
		}
	}
}
