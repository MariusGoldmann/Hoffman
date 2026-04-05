using Unity.VisualScripting;
using UnityEngine;

public class NeedleObstacle : MonoBehaviour
{
    [SerializeField] float damageCooldown = 1;
    float timer;

    CircleCollider2D needleCollider;
    PlayerHealth playerHealth;

    

    void Start()
    {
        needleCollider = GetComponent<CircleCollider2D>();
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
                playerHealth.ChangeHealth(-3, Vector2.zero, Vector2.zero, 1, 1);
                timer = damageCooldown;
            }
        }
    }
}
