using Unity.VisualScripting;
using UnityEngine;

public class NeedleObstacle : MonoBehaviour
{
    [SerializeField] float damageCooldown = 1;
    float timer;

    PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timer < 0)
            {
                Debug.Log("Needle Hit");
                playerHealth.ChangeHealth(-15, Vector2.zero, Vector2.zero, 1, 1, collision.transform.position, false);
                timer = damageCooldown;
            }
        }
    }
}
