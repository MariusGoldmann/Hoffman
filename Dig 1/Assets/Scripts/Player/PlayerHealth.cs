using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 20;
    [SerializeField] float playerHealTickSpeed = 0.5f;
    [HideInInspector] public bool healOverTime;

    int currentPlayerHealth;


    private void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
    }

    public void ChangeHealth(int amount)
    { 
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);

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
            if (currentPlayerHealth>maxPlayerHealth)
            {
                currentPlayerHealth += healPerTick;
                yield return new WaitForSeconds(playerHealTickSpeed);
            }
        }
    }
}
