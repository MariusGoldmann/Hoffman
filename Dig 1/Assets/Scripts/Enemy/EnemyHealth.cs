using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;

    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 
    }

    public void ChangeHealth(int amount)
    {
        currentEnemyHealth += amount;

        if (currentEnemyHealth > maxEnemyHealth)
        {
            currentEnemyHealth = maxEnemyHealth;
        }
        else if (currentEnemyHealth <= 0)
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
    }

}
