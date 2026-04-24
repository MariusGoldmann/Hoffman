using UnityEngine;

public class PlayerSoundFXManager : MonoBehaviour {
	public static PlayerSoundFXManager instance;

	[SerializeField] private AudioClip[] soundEffectList;
	private                  AudioSource audioSource;

	void Awake() {
		if (instance != null) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySound(SoundType sound, float volume = 1) {
		instance.audioSource.PlayOneShot(instance.soundEffectList[(int)sound], volume);
	}

	public enum SoundType {
		WALK,
		RUN,
		JUMP,
		SLASH,
		KICK,
		BOOMERANGTHROW,
		BOOMERANGRETURN,
		SLASHHIT,
		KICKHIT,
		BELL,
		PUMPKIN,
		BUTTON,
		FIREBALL
	}
}