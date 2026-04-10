using Cinemachine;
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

    KnockbackScript knockbackScript;
    LevelLoader levelLoader;

    CinemachineImpulseSource impulseSource;

    [SerializeField] Slider healthSlider; 

    private void Start()
    {
        knockbackScript = GetComponent<KnockbackScript>();
        levelLoader = FindAnyObjectByType<LevelLoader>();

        impulseSource = GetComponent<CinemachineImpulseSource>();

        currentPlayerHealth = maxPlayerHealth;

        healthSlider.maxValue = maxPlayerHealth;
        healthSlider.value = currentPlayerHealth;
    }
    private void Update()
    {

    }
    public void ChangeHealth(int amount, Vector2 hitDirection, Vector2 additionalForceDireciton, float hitDirectionForce, float additionalForce)
    {
        if (CameraShakeManager.instance!=null) CameraShakeManager.instance.CameraShake(impulseSource);
        //Dennis suger 2 was here :D 
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);
        StartCoroutine(knockbackScript.KnockbackAction(hitDirection, additionalForceDireciton, hitDirectionForce, additionalForce));
        if (healthSlider!=null) healthSlider.value = currentPlayerHealth; 
        if (currentPlayerHealth <= 0) StartCoroutine(Deathsequence());
    }


    IEnumerator Deathsequence()
    {
        levelLoader.FadeOut();
        yield return new WaitForSeconds(2);
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
