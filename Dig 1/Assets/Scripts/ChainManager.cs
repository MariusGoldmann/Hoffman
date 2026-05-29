using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChainManager: MonoBehaviour
{
    [SerializeField] Image slashChain;
    [SerializeField] Image kickChain;
    [SerializeField] Image boomerangChain;

    void Update()
    {
        if (SpawnManager.instance.legOwned)
        {
            slashChain.enabled = false;
            kickChain.enabled = false;
            Debug.Log("DEnnis Surer");
        }

        if (SpawnManager.instance.boomerangOwned)
        {
            boomerangChain.enabled = false;
        }
    }

}
