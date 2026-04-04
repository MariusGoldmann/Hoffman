using UnityEngine;

public class InkTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.ChangeHealth(-100, Vector2.zero, Vector2.zero, 0, 0);
        }
    }
}
