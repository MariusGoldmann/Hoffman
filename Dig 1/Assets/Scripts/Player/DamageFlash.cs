using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashTime = 0.25f;
    [SerializeField] AnimationCurve flashSpeedCurve;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material material;

    Coroutine damageFlasherCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / flashTime));

            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    void SetFlashColor()
    {
        material.SetColor("_FlashColor", flashColor);
    }

    void SetFlashAmount(float amount)
    {
        material.SetFloat("_FlashAmount", amount);
    }

    public void GetDamageFlasher()
    {
        damageFlasherCoroutine = StartCoroutine(DamageFlasher());
    }
}
