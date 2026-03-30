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

    [Header("Debug")]
    float normalRadius;
    bool poison;
    bool isExpanding=false;

    LayerMask playerLayer;
    PlayerHealth playerHealth;
    

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        normalRadius = bodyCollider.radius;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !knockbackScript.GetIsKnockback())
        {
            playerHealth.ChangeHealth(-collisionDamage, (other.transform.position - transform.position).normalized, Vector2.up, hitDirectionForce, additionalDirectionalForce);
            if (!poison)
            {
                StartCoroutine(Poison());
            }
            else
            {
                StopCoroutine(Poison());
                StartCoroutine(Poison());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isExpanding) StartCoroutine(Expand());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) StartCoroutine(Shrink());
    }
    IEnumerator Expand()
    {
        isExpanding = true;
        StopCoroutine(Shrink());
        //Animation for visual
        while (bodyCollider.radius<expandedRadius)
        {
            bodyCollider.radius += expandedRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Shrink()
    {
        yield return new WaitForSeconds(maxTimeExpanded);
        isExpanding = false;
        while (bodyCollider.radius > normalRadius)
        {
            bodyCollider.radius -= normalRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Poison()
    {
        poison = true;
        for (int i = 0; i < poisonTickAmount; i++)
        {
            yield return new WaitForSeconds(poisionTickSpeed);
            playerHealth.ChangeHealth(-poisonTickDamage, Vector2.zero, Vector2.zero, 0.5f, 0);
        }
        poison = false;
    }
    public bool GetIsExpanding()
    {
        return isExpanding;
    }
}
