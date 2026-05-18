using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyMenuUi : MonoBehaviour {
	[SerializeField] private GameObject menuImage;

	public void InsertButton() {
		if (!SpawnManager.instance.keyOwned) return;
		CatsleOpener.instance.gateOpened = true;
		menuImage.SetActive(false);
	}
	
	public void NoButton() {
		menuImage.SetActive(false);
	}
}
