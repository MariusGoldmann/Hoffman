using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth = 5;

    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] DamageFlash damageFlash;
    [SerializeField] KnockbackScript knockbackScript;

    void Awake()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        knockbackScript = GetComponent<KnockbackScript>();
        damageFlash = GetComponent<DamageFlash>();
    }
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth; 
    }

    public void ChangeHealth(int amount, Vector2 knockbackdirection)
    {
        StartCoroutine(knockbackScript.KnockbackAction(knockbackdirection, Vector2.up));
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

        damageFlash.GetDamageFlasher();
    }
}
