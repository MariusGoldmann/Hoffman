using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnManager.instance.spawnPosition = transform.position;
            Destroy(gameObject);
        }
    }
}
