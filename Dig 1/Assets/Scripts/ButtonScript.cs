
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

    //  Skapa en delay när resume e klickad fade ut eller liknande. 
    public void OnButtonPressed()
    {
        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BUTTON, 1);
    }

    public void ResumeGame()
    {
        pauseManager.ResumeGame();
    }
}
