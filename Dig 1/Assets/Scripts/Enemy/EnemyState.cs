using UnityEngine;

public class EnemyState : MonoBehaviour
{
    
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform enemyPosition;
    [SerializeField] Rigidbody2D enemyParentRB;
    [SerializeField] EnemyMovement enemyMovement;

    [Header("Debug")]
    int playerDirection;
    bool inCombat;

    private void Update()
    {
        if (enemyPosition.position.x < playerTransform.position.x)
        {
            playerDirection = 1;
        }
        else
        {
            playerDirection = -1;
        }
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


    public int GetPlayerDirection()
    {
        return playerDirection;
    }
}