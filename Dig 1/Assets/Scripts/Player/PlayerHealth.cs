using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 20;
    /*[SerializeField] float playerHealTickSpeed = 0.5f;
    [SerializeField] int playerHealTickAmount = 1;
    [HideInInspector] public bool healOverTime;*/

    [SerializeField] int currentPlayerHealth;


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

/*    private void Update()
    {
        if (healOverTime) StartCoroutine(HealOverTime());
    }


    IEnumerator HealOverTime()
    {
        while (healOverTime && currentPlayerHealth < maxPlayerHealth)
        {
            currentPlayerHealth += playerHealTickAmount;
            yield return new WaitForSeconds(playerHealTickSpeed);
        }
    }*/
}
