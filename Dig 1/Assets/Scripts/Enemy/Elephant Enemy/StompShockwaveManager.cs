using UnityEngine;

public class StompShockwaveManager : MonoBehaviour
{
    [SerializeField] bool left;
    [SerializeField] GameObject[] colliderArray;

    float colliders;

    ObjectPooling stompPool;

    private void Start()
    {
        if (left)
        {
            stompPool = GameObject.FindGameObjectWithTag("StompPoolLeft").GetComponent<ObjectPooling>();
        }
        else
        {
            stompPool = GameObject.FindGameObjectWithTag("StompPoolRight").GetComponent<ObjectPooling>();
        }
        colliders = colliderArray.Length;
    }
    private void OnEnable()
    {
        foreach (GameObject collider in colliderArray)
        {
            collider.SetActive(true);
        }
    }
    private void Update()
    {
        bool allCollidersInactive = true;
        foreach (GameObject collider in colliderArray)
        {
            if (collider.activeInHierarchy)
            {
                allCollidersInactive = false;
                return;
            }
            if (allCollidersInactive == false) return;
            allCollidersInactive = true;
        }
        if (allCollidersInactive) stompPool.ReturnObject(gameObject);
    }
}
