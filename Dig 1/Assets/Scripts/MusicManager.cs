using UnityEngine;

public class MusicManager : MonoBehaviour
{
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
}
