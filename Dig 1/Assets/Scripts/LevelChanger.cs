using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {
	[SerializeField] private int desiredLevel;
	
	[SerializeField] private LevelLoader levelLoader;
	Coroutine levelChangeCoroutine;

	private void Awake() {
		levelLoader = FindFirstObjectByType<LevelLoader>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			if (levelChangeCoroutine != null) return;
				levelChangeCoroutine = StartCoroutine(LevelChangeCoroutine());
		}
	}
	
	private IEnumerator LevelChangeCoroutine()
	{
		levelLoader.FadeOut();
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(desiredLevel);
		levelChangeCoroutine = null;
	}
}
