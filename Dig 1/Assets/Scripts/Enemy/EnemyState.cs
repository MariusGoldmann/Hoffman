using UnityEngine;

public class EnemyState : MonoBehaviour
{
    
    [SerializeField] UnityEngine.Transform playerTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] EnemyMovement enemyMovement;

    [Header("Debug")]
    Vector2 playerDirection;
    [SerializeField] bool inCombat;

    private void Update()
    {
        playerDirection = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
        {
            inCombat = true;
        }
       /*if (collision.CompareTag("Wall"))
        {
            enemyMovement.TurnAround();
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inCombat = false;
        }
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