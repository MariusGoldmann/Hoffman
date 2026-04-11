using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 100;
    [SerializeField] float playerHealTickSpeed = 0.5f;
    [HideInInspector] public bool healOverTime;

    [SerializeField] int currentPlayerHealth;

    KnockbackScript knockbackScript;
    DamageFlash damageFlash;
    LevelLoader levelLoader;

    [SerializeField] CinemachineImpulseSource impulseSource;
    Animator animator;

    [SerializeField] Slider healthSlider;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem healthPickupParticles;

    bool dead;

    private void Start()
    {
        knockbackScript = GetComponent<KnockbackScript>();
        damageFlash=GetComponent<DamageFlash>();
        levelLoader = FindAnyObjectByType<LevelLoader>();

        impulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponentInChildren<Animator>();

        currentPlayerHealth = maxPlayerHealth;

        healthSlider.maxValue = maxPlayerHealth;
        healthSlider.value = currentPlayerHealth;
    }
    public void ChangeHealth(int amount, Vector2 hitDirection, Vector2 additionalForceDireciton, float hitDirectionForce, float additionalForce, Vector3 collisionPoint)
    {
        if (CameraShakeManager.instance!=null) CameraShakeManager.instance.CameraShake(impulseSource);
        //Dennis suger 2 was here :D 
        damageFlash.GetDamageFlasher();
        hitParticles.transform.position = collisionPoint;
        hitParticles.Play();
        currentPlayerHealth += amount;
        Mathf.Clamp(currentPlayerHealth, float.MinValue, maxPlayerHealth);
        if (currentPlayerHealth>0) StartCoroutine(knockbackScript.KnockbackAction(hitDirection, additionalForceDireciton, hitDirectionForce, additionalForce));
        if (healthSlider!=null) healthSlider.value = currentPlayerHealth;
        if (currentPlayerHealth <= 0) StartCoroutine(Deathsequence());
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("1");
        if (other.gameObject.CompareTag("HpParticle"))
        {
            Debug.Log("2");
            currentPlayerHealth += 10;
            healthPickupParticles.Play();
            Destroy(other.gameObject);
        }
    }
    IEnumerator Deathsequence()
    {
        dead = true;
        animator.SetTrigger("Dying");
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
    public bool GetIsDead()
    {
        return dead;
    }
}
