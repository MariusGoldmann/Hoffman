using Unity.VisualScripting;
using UnityEngine;

public class BlueTile : MonoBehaviour
{
    SpawnManager spawnManager;
    SpriteRenderer spriteRenderer;
    Collider2D collider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        spawnManager = FindFirstObjectByType<SpawnManager>();

        spriteRenderer.enabled = false;
        collider.enabled = false;
    }

    private void Update()
    {
        if (spawnManager.eyeOwned == true)
        {
            spriteRenderer.enabled = true;
            collider.enabled = true;
        }
    }
}
