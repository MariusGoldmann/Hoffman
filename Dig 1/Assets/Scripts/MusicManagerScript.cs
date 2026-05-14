using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManagerScript : MonoBehaviour
{
	private static MusicManagerScript instance;
	
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip   mainMenuMusic;
	[SerializeField] private AudioClip   mainGameMusic;
	[SerializeField] private AudioClip   castleMusic;
	
    private void Awake()
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
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
	    MusicSwitcher();
    }

    private void MusicSwitcher() {
	    audioSource.clip = SceneManager.GetActiveScene().name switch {
		    "StartScene"   => mainMenuMusic,
		    "Table Scene"  => mainGameMusic,
		    "Shelf Scene"  => mainGameMusic,
		    "Castle Scene" => castleMusic,
		    _              => mainGameMusic
	    };
	    if (!audioSource.isPlaying) audioSource.Play();
    }
}
