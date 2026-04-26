
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    private Button buttonElement;

    PauseManager pauseManager;

    public void Start()
    {
        buttonElement = GetComponent<Button>();
        buttonElement.onClick.AddListener(OnButtonPressed);
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    public void OnButtonPressed()
    {
        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BUTTON, 1);
    }

    public void ResumeGame()
    {
        pauseManager.ResumeGame();
    }
}
