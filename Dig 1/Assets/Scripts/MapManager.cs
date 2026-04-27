using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
	private static MapManager instance;

	[SerializeField] private float fadeSpeed;

	[Header("Map Settings for bookshelf")]
	[SerializeField] private GameObject mapImage;
	[SerializeField] private Image pinkShadow;
	[SerializeField] private Image redShadow;
	[SerializeField] private Image yellowShadow;
	[SerializeField] private Image blueShadow;
	[SerializeField] private Image hiddenPassage;

	[SerializeField] private CanvasGroup pinkCanvasGroup;
	[SerializeField] private CanvasGroup redCanvasGroup;
	[SerializeField] private CanvasGroup yellowCanvasGroup;
	[SerializeField] private CanvasGroup blueCanvasGroup;
	[SerializeField] private CanvasGroup hiddenCanvasGroup;
	
	public bool pinkMapTriggered;
	public bool redMapTriggered;
	public bool yellowMapTriggered;
	public bool blueMapTriggered;
	public bool hiddenMapTriggered;

	[SerializeField] private bool mapOpened;


	private void Awake() {
		mapImage.SetActive(false);
		if (instance != null) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(gameObject);

		pinkCanvasGroup   = pinkShadow.GetComponent<CanvasGroup>();
		redCanvasGroup    = redShadow.GetComponent<CanvasGroup>();
		yellowCanvasGroup = yellowShadow.GetComponent<CanvasGroup>();
		blueCanvasGroup   = blueShadow.GetComponent<CanvasGroup>();
		hiddenCanvasGroup = hiddenPassage.GetComponent<CanvasGroup>();
	}

	private void Update() {
		if (Keyboard.current.mKey.wasPressedThisFrame) {
			mapOpened = !mapOpened;
		}

		mapImage.SetActive(mapOpened);

		Time.timeScale = mapOpened ? 0 : 1;
		if (pinkMapTriggered) {
			ShadowFader(pinkShadow, pinkCanvasGroup, 0);
		}
		if (redMapTriggered) {
			ShadowFader(redShadow, redCanvasGroup, 0);
		}
		if (yellowMapTriggered) {
			ShadowFader(yellowShadow, yellowCanvasGroup, 0);
		}
		if (blueMapTriggered) {
			ShadowFader(blueShadow, blueCanvasGroup, 0);
		}
		if (hiddenMapTriggered) {
			ShadowFader(hiddenPassage, hiddenCanvasGroup, 1);
		}
	}

	private void ShadowFader(Image shadowImage, CanvasGroup shadowCanvasGroup, float desiredAlpha) {
		if (mapOpened) {
			if (!shadowCanvasGroup.gameObject.LeanIsTweening()) {
				LeanTween.alphaCanvas(shadowCanvasGroup, desiredAlpha, fadeSpeed).setEaseInOutSine().setIgnoreTimeScale(true);
			}
		}
		
		if (shadowCanvasGroup.alpha <= 0) {
			shadowImage.gameObject.SetActive(false);
		}
	}
}