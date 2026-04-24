using System;
using UnityEngine;

public class TrumpetProjectile : MonoBehaviour
{
    [SerializeField] int damage=7;
    [SerializeField] float timeUntilDeath = 3f;
    [SerializeField] float speed = 10f;

    float elapsedTime;
    Vector2 initialDirection;
    Vector2 direction;

    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ElephantCombat elephantCombat;
    Rigidbody2D projectileRB;

    private void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        initialDirection = elephantCombat.GetInitialDirection();
        direction = new Vector2(1, 1);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeUntilDeath)
        {
            trumpetPool.ReturnObject(gameObject);
            elapsedTime = 0;
        }
        Vector2 finalDirection = initialDirection * direction;
        Debug.Log(direction);
        projectileRB.linearVelocity = finalDirection * speed;
        transform.rotation = Quaternion.LookRotation(finalDirection);
        Debug.Log(finalDirection);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(damage, other.transform.position - transform.position, Vector2.up, 10f, 5f, other.GetContact(0).point, false);
            trumpetPool.ReturnObject(gameObject);
        }
        else
        {
            direction=(transform.position - other.transform.position).normalized;
        }
    }
}
