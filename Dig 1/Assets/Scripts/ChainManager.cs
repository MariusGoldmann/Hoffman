using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChainManager: MonoBehaviour
{
    public static ChainManager instance;

    SpawnManager spawnManager;
    [SerializeField] Image slashChain;
    [SerializeField] Image kickChain;
    [SerializeField] Image boomerangChain;

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
        spawnManager = FindAnyObjectByType<SpawnManager>();
    }

    void Update()
    {
        if (spawnManager.legOwned)
        {
            slashChain.enabled = false;
            kickChain.enabled = false;
        }

        if (spawnManager.boomerangOwned)
        {
            boomerangChain.enabled = false;
        }
    }

}
