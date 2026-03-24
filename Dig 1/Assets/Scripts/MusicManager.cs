using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider; 

    private void Awake()
    {
        int numberOfMusicManager = FindObjectsByType<MusicManager>(FindObjectsSortMode.None).Length;

        if (numberOfMusicManager > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
