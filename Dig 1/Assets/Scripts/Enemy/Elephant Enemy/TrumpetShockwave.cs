using System.Collections;
using UnityEngine;

public class TrumpetShockwave : MonoBehaviour
{
    [SerializeField] int damage=7;
    [SerializeField] float timeUntilDeath = 3f;
    [SerializeField] float speed = 10f;
    [SerializeField] float hitDirectionForce = 10f;
    [SerializeField] float additionalForce = 5f;

    float elapsedTime;
    Vector2 direction;
    Coroutine deleteCoroutine;

    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Rigidbody2D projectileRB;
    ObjectPooling trumpetPool;

    private void OnEnable()
    {
        trumpetPool = GameObject.FindGameObjectWithTag("TrumpetPool").GetComponent<ObjectPooling>();

        projectileRB.linearVelocity = Vector2.zero;
        elapsedTime = 0;
    }
    public void SetInitialDirection(Vector2 initialDirection)
    {
        direction = initialDirection;
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeUntilDeath && deleteCoroutine == null) trumpetPool.ReturnObject(gameObject);
    }
    private void FixedUpdate()
    {
        projectileRB.linearVelocity = direction * speed;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.rotation = rotation;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage, other.transform.position - transform.position, Vector2.up, hitDirectionForce, additionalForce, other.GetContact(0).point, false);
            deleteCoroutine = StartCoroutine(Delete(other));
        }
        else
        {
            direction = Vector2.Reflect(direction, other.GetContact(0).normal);
        }
    }
    IEnumerator Delete(Collision2D other)
    {
        Vector2 particleLocation = other.GetContact(0).point;
        Quaternion particleAngle = Quaternion.Euler(0, 0, Mathf.Atan2(other.GetContact(0).point.y - transform.position.y, other.GetContact(0).point.x - transform.position.x) * Mathf.Rad2Deg);
        
        deathParticles.transform.position = particleLocation;
        deathParticles.transform.rotation = particleAngle;
        deathParticles.Play();

        yield return new WaitForSeconds(0.05f);
        deleteCoroutine = null;
        trumpetPool.ReturnObject(gameObject);
    }
}