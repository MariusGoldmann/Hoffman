using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [Header("Volume Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundFXSlider;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource soundFXAudioSource;

    void Awake()
    {

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

        if (!PlayerPrefs.HasKey("soundFXVolume"))
        {
            PlayerPrefs.SetFloat("soundFXVolume", 1);
        }
        else
        {
            Load();
        }

        if (musicAudioSource == null)
        {           
            musicAudioSource = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
        }


        if (soundFXAudioSource == null)
        {
            soundFXAudioSource = GameObject.FindGameObjectWithTag("SoundFXAudioSource").GetComponent<AudioSource>();
        }
    }
    void FixedUpdate()
    {
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        musicAudioSource.volume = musicSlider.value;
        soundFXAudioSource.volume = soundFXSlider.value;
        Save();
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("soundFXVolume", soundFXSlider.value);
    }
}
