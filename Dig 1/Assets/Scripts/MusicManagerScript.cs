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
	    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) {
		    audioSource.clip = mainMenuMusic;
	    }

	    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
		    audioSource.clip = mainGameMusic;
	    }

	    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
		    audioSource.clip = mainMenuMusic;
	    }

	    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3)) {
		    audioSource.clip = castleMusic;
	    }
    }
}
