using UnityEngine;

public class InkTile : MonoBehaviour
{
    PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.ChangeHealth(-100, Vector2.zero, Vector2.zero, 0, 0, other.GetContact(0).point, false);
        }
    }
}
