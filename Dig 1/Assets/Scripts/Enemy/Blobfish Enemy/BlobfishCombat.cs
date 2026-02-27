using System.Collections;
using UnityEngine;

public class BlobfishCombat : MonoBehaviour
{
    [Header("Expansion")]
    [SerializeField] CircleCollider2D bodyCollider;
    [SerializeField] float expandedRadius = 2f;
    [SerializeField] int collisionDamage = 2;
    [SerializeField] int maxTimeExpanded = 2;

    [Header("Poison")]
    [SerializeField] int poisonTickDamage = 1;
    [SerializeField] int poisonTickAmount = 3;
    [SerializeField] float poisionTickSpeed = 1;

    bool poison;

    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) StartCoroutine(Expand());
    }
    IEnumerator Expand()
    {
        //Animation
        for (float f = 0; f < expandedRadius; f = bodyCollider.radius)
        {
            bodyCollider.radius += expandedRadius * Time.deltaTime;
            Debug.Log(f);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.ChangeHealth(-collisionDamage);
            if (!poison) StartCoroutine(Poison());
        }
    }

    IEnumerator Poison()
    {
        poison = true;
        for (int i = 0; i < poisonTickAmount; i++)
        {
            yield return new WaitForSeconds(poisionTickSpeed);
            playerHealth.ChangeHealth(-poisonTickDamage);
        }
        poison = false;
    }
}
