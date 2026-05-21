using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class StompShockwave : MonoBehaviour
{
    [SerializeField] bool left;
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float timeUntilDeath;
    [SerializeField] float hitDirectionForce = 6f;
    [SerializeField] float additionalForce = 3f;

    float direction = 1;
    float elapsedTime;
    Vector2 originalPosition;

    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Rigidbody2D projectileRB;

    private void Awake()
    {
        if (left) direction = -1;
        originalPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        transform.localPosition = originalPosition;
        projectileRB.linearVelocity = Vector2.zero;
        elapsedTime = 0;
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeUntilDeath) gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        projectileRB.linearVelocity = new Vector2(direction * speed, 0);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage, other.transform.position - transform.position, Vector2.up, hitDirectionForce, additionalForce, other.GetContact(0).point, false);
            StartCoroutine(Delete(other));
        }
        else StartCoroutine(Delete(other));
    }
    IEnumerator Delete(Collision2D other)
    {
        Vector2 particleLocation = other.GetContact(0).point;
        Quaternion particleAngle = Quaternion.Euler(0, 0, Mathf.Atan2(other.GetContact(0).point.y - transform.position.y, other.GetContact(0).point.x - transform.position.x) * Mathf.Rad2Deg + 90);

        deathParticles.transform.position = particleLocation;
        deathParticles.transform.rotation = particleAngle;
        deathParticles.Play();

        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
