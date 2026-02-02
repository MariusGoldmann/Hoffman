using UnityEngine;

public class EnemyState : MonoBehaviour
{
    
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] EnemyMovement enemyMovement;

    [Header("Debug")]
    Vector2 playerDirection;
    bool inCombat;

    private void Update()
    {
        playerDirection = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player")) inCombat = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inCombat = false;
    }


    public bool GetInCombat()
    {
        return inCombat;
    }


    public Vector2 GetPlayerDirection()
    {
        return playerDirection;
    }
}