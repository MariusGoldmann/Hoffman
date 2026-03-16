using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 20;
    [SerializeField] float playerHealTickSpeed = 0.5f;
    [HideInInspector] public bool healOverTime;

    [SerializeField] int currentPlayerHealth;

    KnockbackScript knockbackScript;
    PlayerMovement playerMovement;

    [SerializeField] Slider healthSlider; 

    private void Start()
    {
        knockbackScript = GetComponent<KnockbackScript>();
        playerMovement= GetComponent<PlayerMovement>();

        currentPlayerHealth = maxPlayerHealth;

        healthSlider.maxValue = maxPlayerHealth;
        healthSlider.value = currentPlayerHealth;
    }
    private void Update()
    {
        Debug.Log(currentPlayerHealth);
    }
    public void ChangeHealth(int amount, Vector2 hitDirection, Vector2 additionalForceDireciton)
    {
        //Dennis suger 2 was here :D 
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);
        StartCoroutine(knockbackScript.KnockbackAction(hitDirection, additionalForceDireciton));
        healthSlider.value = currentPlayerHealth; 
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
