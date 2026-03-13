using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 20;
    [SerializeField] float playerHealTickSpeed = 0.5f;
    [HideInInspector] public bool healOverTime;

    [SerializeField] int currentPlayerHealth;

    [SerializeField] public Slider HealthBar;

    
    private void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        HealthBar.maxValue = maxPlayerHealth;
        HealthBar.value = currentPlayerHealth;
    }

    public void ChangeHealth(int amount)
    { 
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);

        HealthBar.value = currentPlayerHealth;

        if (currentPlayerHealth <= 0) DeathSequence();
    }

    void DeathSequence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator HealOverTime(int tickAmount, int healPerTick)
    {
        for (int i=0; i<tickAmount; i++)
        {
            if (currentPlayerHealth<maxPlayerHealth)
            {
                currentPlayerHealth += healPerTick;
                yield return new WaitForSeconds(playerHealTickSpeed);
            }
        }
    }
}
