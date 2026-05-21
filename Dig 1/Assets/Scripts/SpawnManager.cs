using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Vector2 spawnPosition;

    [SerializeField] bool reloadScene = false;
    public bool legOwned = false;
    public bool eyeOwned = false;
    public bool boomerangOwned = false;
    public bool earOwned = false;
    public bool keyOwned = false;

    [SerializeField] GameObject legPickUp;
    [SerializeField] GameObject eyePickup;
    [SerializeField] GameObject earPickUp;
    [SerializeField] GameObject boomerangPickUp;
	[SerializeField] GameObject keyPickUp;

    PickUpScript pickUpScript;
    PlayerHealth playerHealth;

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
        spawnPosition =  transform.position;

        pickUpScript = FindFirstObjectByType<PickUpScript>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        legPickUp = GameObject.FindGameObjectWithTag("PlayerLeg");
        eyePickup = GameObject.FindGameObjectWithTag("PlayerEye");
        earPickUp = GameObject.FindGameObjectWithTag("PlayerEar");
        boomerangPickUp = GameObject.FindGameObjectWithTag("BoomerangPickUp");
        keyPickUp = GameObject.FindGameObjectWithTag("Key");

        SceneReloader();
        PickUpManager();
        PickupDestroy();
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
        if (pickUpScript.GetHasEar() == true)
        {
            earOwned = true;
        }

        if (pickUpScript.GetHasKey() == true) {
	        keyOwned = true;
        }
    }

    void PickupDestroy()
    {
        if (legOwned)
        {
            Destroy(legPickUp);
        }
        if (eyeOwned)
        {
            Destroy(eyePickup);
        }
        if (earOwned)
        {
            Destroy(earPickUp);
        }
        if (boomerangOwned)
        {
            Destroy(boomerangPickUp);
        }

        if (keyOwned) {
	        Destroy(keyPickUp);
        }
    }
}
