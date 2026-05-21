using TMPro;
using UnityEngine;

public class KeyMenuUi : MonoBehaviour {
	
	[SerializeField] private TextMeshProUGUI insertKeyText;
	[SerializeField] private string          insertKey  = "Insert Key";
	[SerializeField] private string          youNeedKey = "You need the key first";
	[SerializeField] private GameObject      menuImage;

	public void InsertButton() {
		if (!SpawnManager.instance.keyOwned) {
			insertKeyText.text = youNeedKey;
			insertKeyText.color = Color.red;
		} else {
			insertKeyText.text               = insertKey;
			insertKeyText.color              = Color.white;
			CatsleOpener.instance.gateOpened = true;
			menuImage.SetActive(false);
		}
	}
	
	public void NoButton() {
		insertKeyText.text  = insertKey;
		insertKeyText.color = Color.antiqueWhite;
		menuImage.SetActive(false);
	}
}
