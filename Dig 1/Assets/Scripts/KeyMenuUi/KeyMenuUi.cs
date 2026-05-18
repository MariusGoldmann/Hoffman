using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyMenuUi : MonoBehaviour {
	[SerializeField] private GameObject menuImage;
	

	
	private void Update() {
		
	}

	public void InsertButton() {
		if (SpawnManager.instance.keyOwned) {
			CatsleOpener.instance.gateOpened = true;
		}
	}
	
	public void NoButton() {
		menuImage.SetActive(false);
	}
}
