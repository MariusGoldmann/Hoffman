using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChainManager: MonoBehaviour
{

    SpawnManager spawnManager;
    [SerializeField] Image slashChain;
    [SerializeField] Image kickChain;
    [SerializeField] Image boomerangChain;

    void Awake()
    {
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
