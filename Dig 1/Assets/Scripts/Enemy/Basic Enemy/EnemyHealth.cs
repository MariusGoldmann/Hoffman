using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;

    [SerializeField] PlayerCombat playerCombat;

    void Awake()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang"))
        {
            Debug.Log("Enemy hit");
            ChangeHealth(-playerCombat.GetBoomerangDamage());

        }
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
