using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        if (spriteRenderer==null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }
    IEnumerator DamageFlasher()
    {
        SetFlashColor();
        Debug.Log("1");
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            Debug.Log("2");
            elapsedTime += Time.deltaTime;

            currentFlashAmount = flashSpeedCurve.Evaluate(elapsedTime/flashTime);

            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
        Debug.Log("3");
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
        Debug.Log("4");
        damageFlasherCoroutine = StartCoroutine(DamageFlasher());
    }
}
