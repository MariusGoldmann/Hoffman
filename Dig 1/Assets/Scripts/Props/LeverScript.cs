using UnityEngine;
using UnityEngine.InputSystem;

public class LeverScript : MonoBehaviour {
	private static readonly  int      Open = Animator.StringToHash("Open");
	private static readonly  int      Pull = Animator.StringToHash("Pull");
	[SerializeField] private Animator doorAnimator;
	[SerializeField] private Animator leverAnimator;

	private void OnTriggerStay2D(Collider2D other) {
		if (!Keyboard.current.eKey.wasPressedThisFrame || !other.CompareTag("Player")) return;
		doorAnimator.SetTrigger(Open);
		leverAnimator.SetTrigger(Pull);
		Debug.Log("Open");
	}
}
