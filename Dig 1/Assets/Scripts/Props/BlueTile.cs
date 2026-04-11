using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class BlueTile : MonoBehaviour
{
    SpawnManager spawnManager;
    SpriteRenderer spriteRenderer;
    Collider2D collider;
    Light2D spotLight;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        spotLight = GetComponentInChildren<Light2D>();

        spriteRenderer.enabled = false;
        collider.enabled = false;
        spotLight.enabled = false;
    }

    private void Update()
    {
        if (spawnManager.eyeOwned == true)
        {
            spriteRenderer.enabled = true;
            collider.enabled = true;
            spotLight.enabled = true;
        }
    }
}
