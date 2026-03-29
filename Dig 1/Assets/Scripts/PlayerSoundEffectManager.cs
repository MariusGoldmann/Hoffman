using UnityEngine;

public class PlayerSoundEffectManager : MonoBehaviour
{
    [SerializeField] AudioClip[] soundEffectList;
    static PlayerSoundEffectManager instance;
    AudioSource audioSource;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundEffectList[(int)sound], volume);
    }

    public enum SoundType
    {
        WALK,
        RUN,
        JUMP,
        SLASH,
        KICK,
        BOOMERANG,
        DAMAGE

    }

}
