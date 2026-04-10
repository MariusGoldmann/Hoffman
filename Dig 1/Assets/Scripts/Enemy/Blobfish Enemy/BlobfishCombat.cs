using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class BlobfishCombat : MonoBehaviour
{
    [Header("Expansion")]
    [SerializeField] CircleCollider2D bodyCollider;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] float expandedRadius = 2f;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalDirectionalForce = 5f;
    [SerializeField] int collisionDamage = 2;
    [SerializeField] int maxTimeExpanded = 2;


    [Header("Poison")]
    [SerializeField] int poisonTickDamage = 1;
    [SerializeField] int poisonTickAmount = 3;
    [SerializeField] float poisionTickSpeed = 1;
   
    [Header("Graphics")]
    [SerializeField] ParticleSystem transitionParticles;
    [SerializeField] Transform bigTransform;
    [SerializeField] Transform smallTransform;
    [SerializeField] SpriteRenderer bigSprite;
    [SerializeField] SpriteRenderer smallSprite;
    [SerializeField] float sizeChangeMultiplier = 2;

    [Header("Debug")]
    float normalRadius;
    bool isBlown=false;
    Coroutine shrinkCoroutine;
    Coroutine poisonCoroutine;

    LayerMask playerLayer;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        normalRadius = bodyCollider.radius;
        bigSprite.enabled=false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !knockbackScript.GetIsKnockback())
        {
            playerHealth.ChangeHealth(-collisionDamage, (other.transform.position - transform.position).normalized, Vector2.up, hitDirectionForce, additionalDirectionalForce);
            if (poisonCoroutine==null)
            {
                poisonCoroutine=StartCoroutine(Poison());
            }
            else
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine=StartCoroutine(Poison());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isBlown) StartCoroutine(Expand());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) shrinkCoroutine = StartCoroutine(Shrink());
    }
    IEnumerator Expand()
    {
        isBlown = true;
        if (shrinkCoroutine!=null) StopCoroutine(shrinkCoroutine);
        StartCoroutine(SpriteSwitch());

        while (bodyCollider.radius<expandedRadius)
        {
            bodyCollider.radius += expandedRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Shrink()
    {
        yield return new WaitForSeconds(maxTimeExpanded);
        StartCoroutine(SpriteSwitch());
        while (bodyCollider.radius > normalRadius)
        {
            bodyCollider.radius -= normalRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isBlown = false;
    }
    IEnumerator Poison()
    {
        for (int i = 0; i < poisonTickAmount; i++)
        {
            yield return new WaitForSeconds(poisionTickSpeed);
            playerHealth.ChangeHealth(-poisonTickDamage, Vector2.zero, Vector2.zero, 0.5f, 0);
        }
    }
    IEnumerator SpriteSwitch()
    {
        float size;
        float originalSize;
        Debug.Log(bigSprite.enabled);

        if (bigSprite.enabled==true)
        {
            originalSize = bigTransform.localScale.x;
            size = bigTransform.localScale.x;
            if (transitionParticles != null) transitionParticles.Play();
            bigSprite.enabled = false;
            smallSprite.enabled = true;
            while (smallTransform.localScale.x>1)
            {
                size -= sizeChangeMultiplier * Time.deltaTime;
                smallTransform.localScale = new Vector3(size, size, 1);
                yield return null;
            }
            bigTransform.localScale = new Vector3(originalSize, originalSize, 1);
        }
        else
        {
            size = smallTransform.localScale.x;
            while (smallTransform.localScale.x<bigTransform.localScale.x)
            {
                size += sizeChangeMultiplier * Time.deltaTime;
                smallTransform.localScale = new Vector3(size, size, 1);
                yield return null;
            }
            if (transitionParticles != null) transitionParticles.Play();
            smallSprite.enabled = false;
            bigSprite.enabled = true;
        }
    }
    public bool GetIsExpanding()
    {
        return isBlown;
    }
    public SpriteRenderer GetActiveSpriteRenderer()
    {
        if (bigSprite.enabled == true) return bigSprite;
        else return smallSprite;
    }
}
