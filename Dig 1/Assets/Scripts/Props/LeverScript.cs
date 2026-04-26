using UnityEngine;
using UnityEngine.InputSystem;

public class LeverScript : MonoBehaviour {
	private static readonly  int      Open = Animator.StringToHash("Open");
	[SerializeField] private Animator doorAnimator;

	private void OnTriggerStay2D(Collider2D other) {
		if (!Keyboard.current.eKey.wasPressedThisFrame || !other.CompareTag("Player")) return;
		doorAnimator.SetTrigger(Open);
		Debug.Log("Open");
	}
}
