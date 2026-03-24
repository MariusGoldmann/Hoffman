using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Vector2 spawnPosition;

    public bool reloadScene = false;
    public bool legOwned = false;
    public bool eyeOwned = false;
    public bool boomerangOwned = false;

    PickUpScript pickUpScript;

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

        pickUpScript = FindFirstObjectByType<PickUpScript>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        SceneReloader();
        PickUpManager();
    }

    void SceneReloader()
    {
        if (reloadScene == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            reloadScene = false;
        }
    }

    void PickUpManager()
    {
        if (pickUpScript.GetHasLeg() == true)
        {
            legOwned = true;
        }
        if (pickUpScript.GetHasEye() == true)
        {
            eyeOwned = true;
        }
        if (pickUpScript.GetHasBoomerang() == true)
        {
            boomerangOwned = true;
        }
    }
}
