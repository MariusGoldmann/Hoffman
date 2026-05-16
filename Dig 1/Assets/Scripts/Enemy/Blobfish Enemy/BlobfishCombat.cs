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
    [SerializeField] float additionalForce = 5f;
    [SerializeField] int collisionDamage = 2;
    [SerializeField] int maxTimeExpanded = 2;

    [Header("Poison")]
    [SerializeField] int poisonTickDamage = 1;
    [SerializeField] int poisonTickAmount = 3;
    [SerializeField] float poisionTickSpeed = 1; 
    
    [Header("Death Sequence")]
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] GameObject hpParticlePrefab;

    [Header("Graphics")]
    [SerializeField] ParticleSystem transitionParticles;
    [SerializeField] Transform bigTransform;
    [SerializeField] Transform smallTransform;
    [SerializeField] SpriteRenderer bigSprite;
    [SerializeField] SpriteRenderer smallSprite;
    [SerializeField] float sizeChangeMultiplier = 2;

    [Header("Debug")]
    float size;
    float sizeLowerBoundary;
    float sizeUpperBoundary;
    float normalRadius;
    bool isBlown=false;
    Coroutine shrinkCoroutine;
    Coroutine poisonCoroutine;

    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        normalRadius = bodyCollider.radius;
        bigSprite.enabled = false;
        sizeLowerBoundary = smallTransform.localScale.x;
        sizeUpperBoundary = 1.6f;
    }
    private void Update()
    {
        Mathf.Clamp(size, sizeLowerBoundary, sizeUpperBoundary);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !knockbackScript.GetIsKnockback())
        {
            playerHealth.ChangeHealth(-collisionDamage, (other.transform.position - transform.position).normalized, Vector2.up, hitDirectionForce, additionalForce, other.GetContact(0).point, false);
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isBlown) StartCoroutine(Expand());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && shrinkCoroutine==null) shrinkCoroutine = StartCoroutine(Shrink());
    }
    IEnumerator Expand()
    {
        isBlown = true;
        StartCoroutine(SpriteSwitch());

        while (bodyCollider.radius<expandedRadius)
        {
            bodyCollider.radius += sizeChangeMultiplier * Time.deltaTime;
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
        shrinkCoroutine = null;
    }
    IEnumerator Poison()
    {
        for (int i = 0; i < poisonTickAmount; i++)
        {
            yield return new WaitForSeconds(poisionTickSpeed);
            playerHealth.ChangeHealth(-poisonTickDamage, Vector2.zero, Vector2.zero, 0, 0, playerHealth.transform.position, true);
        }
    }
    IEnumerator SpriteSwitch()
    {
        if (bigSprite.enabled==true)
        {
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
    public IEnumerator DeathSequence()
    {
        Debug.Log("started");
        Instantiate(hpParticlePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        hitParticles.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    public bool GetIsExpanding()
    {
        return isBlown;
    }
}
