using System;
using UnityEngine;

public class MapTriggerData : MonoBehaviour
{
	private static MapTriggerData instance;
	private MapManager mapManager;

	private void Awake() {
		if (instance != null) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(gameObject);
		mapManager = FindAnyObjectByType<MapManager>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("PinkMapTrigger")) {
			mapManager.pinkMapTriggered = true;
		} 
		if (other.CompareTag("RedMapTrigger")) {
			mapManager.redMapTriggered = true;
		} 
		if (other.CompareTag("YellowMapTrigger")) {
			mapManager.yellowMapTriggered = true;
		} 
		if (other.CompareTag("BlueMapTrigger")) {
			mapManager.blueMapTriggered = true;
		} 
		if (other.CompareTag("HiddenMapTrigger")) {
			mapManager.hiddenMapTriggered = true;
		} 
	}
}