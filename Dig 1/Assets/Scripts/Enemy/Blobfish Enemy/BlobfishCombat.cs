using System.Collections;
using UnityEngine;

public class BlobfishCombat : MonoBehaviour
{
    [Header("Expansion")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] int collisionDamage = 2;

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
        if (other.gameObject.CompareTag("Player")) Expand();
        Debug.Log("Dennis suger 1");
    }
    void Expand()
    {

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
