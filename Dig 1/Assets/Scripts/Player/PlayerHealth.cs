using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 20;
    [SerializeField] float playerHealTickSpeed = 0.5f;
    [HideInInspector] public bool healOverTime;

    [SerializeField] int currentPlayerHealth;

    KnockbackScript knockbackScript;
    PlayerMovement playerMovement;

    private void Start()
    {
        knockbackScript = GetComponent<KnockbackScript>();
        playerMovement= GetComponent<PlayerMovement>();

        currentPlayerHealth = maxPlayerHealth;
    }
    private void Update()
    {
        Debug.Log(currentPlayerHealth);
    }
    public void ChangeHealth(int amount, Vector2 hitDirection, Vector2 additionalForceDireciton)
    {
        Debug.Log("Dennis suger 2");
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);
        StartCoroutine(knockbackScript.KnockbackAction(hitDirection, additionalForceDireciton));
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
