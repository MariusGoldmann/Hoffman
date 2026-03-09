using System.Collections;
using UnityEngine;
using UnityEngine.iOS;

public class BlobfishCombat : MonoBehaviour
{
    [Header("Expansion")]
    [SerializeField] CircleCollider2D bodyCollider;
    [SerializeField] Transform playerTransform;
    [SerializeField] float expandedRadius = 2f;
    [SerializeField] int collisionDamage = 2;
    [SerializeField] int maxTimeExpanded = 2;

    [Header("Poison")]
    [SerializeField] int poisonTickDamage = 1;
    [SerializeField] int poisonTickAmount = 3;
    [SerializeField] float poisionTickSpeed = 1;

    [Header("Debug")]
    float normalRadius;
    bool poison;
    [SerializeField] bool expanding=false;
    [SerializeField] bool shrinking=false;

    LayerMask playerLayer;
    PlayerHealth playerHealth;
    KnockbackScript knockbackScript;
    

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        knockbackScript = GetComponent<KnockbackScript>();

        normalRadius = bodyCollider.radius;
    }

    private void FixedUpdate()
    {
        if (bodyCollider.IsTouchingLayers(playerLayer) && !knockbackScript.GetIsKnockback())
        {
            Debug.Log("Dennis suger 1");
            Vector2 playerDirection = (playerTransform.position - transform.position).normalized;
            playerHealth.ChangeHealth(-collisionDamage, playerDirection);
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
        if (other.gameObject.CompareTag("Player") && !expanding) StartCoroutine(Expand());
    }
    IEnumerator Expand()
    {
        expanding = true;
        shrinking = false;
        StopCoroutine(Shrink());
        //Animation for visual
        while (bodyCollider.radius<expandedRadius)
        {
            bodyCollider.radius += expandedRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(maxTimeExpanded);
        StartCoroutine(Shrink());
        expanding = false;
    }
    IEnumerator Shrink()
    {
        shrinking = true;
        while (bodyCollider.radius > normalRadius)
        {
            bodyCollider.radius -= normalRadius * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        shrinking = false;
    }

    IEnumerator Poison()
    {
        poison = true;
        for (int i = 0; i < poisonTickAmount; i++)
        {
            yield return new WaitForSeconds(poisionTickSpeed);
            playerHealth.ChangeHealth(-poisonTickDamage, Vector2.zero);
        }
        poison = false;
    }
}
