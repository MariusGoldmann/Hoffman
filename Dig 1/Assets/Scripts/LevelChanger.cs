using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {
	[SerializeField] private int desiredLevel;
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player"))
		{
			SceneManager.LoadScene(desiredLevel);
		}
	}
}
