using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	private static MapManager instance;

	[Header("Map Settings")]
	[SerializeField] private GameObject mapImage;
	[SerializeField] private Image pinkShadow;
	[SerializeField] private Image redShadow;
	[SerializeField] private Image yellowShadow;
	[SerializeField] private Image blueShadow;
	[SerializeField] private Image hiddenPassage;
	
	[SerializeField] private bool mapOpened;


	private void Awake() {
		mapImage.SetActive(false);
		if (instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Update() {
		if (Keyboard.current.mKey.wasPressedThisFrame) {
			mapOpened = !mapOpened;
		}
		mapImage.SetActive(mapOpened);

		Time.timeScale = mapOpened ? 0 : 1;
	}
}
