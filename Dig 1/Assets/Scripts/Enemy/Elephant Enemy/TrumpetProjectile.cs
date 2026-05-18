using System;
using UnityEngine;

public class TrumpetProjectile : MonoBehaviour
{
    [SerializeField] int damage=7;
    [SerializeField] float timeUntilDeath = 3f;
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationOffset = 180;

    float elapsedTime;
    Vector2 initialDirection;
    Vector2 finalDirection;

    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ElephantCombat elephantCombat;
    Rigidbody2D projectileRB;

    private void OnEnable()
    {
        projectileRB = GetComponent<Rigidbody2D>();

        initialDirection = elephantCombat.GetInitialDirection();
        finalDirection = initialDirection;
        projectileRB.linearVelocity = Vector2.zero;

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeUntilDeath)
        {
            trumpetPool.ReturnObject(gameObject);
            elapsedTime = 0;
        }
        projectileRB.linearVelocity = finalDirection * speed;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg + rotationOffset);
        Debug.Log(Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg+rotationOffset);
        transform.rotation = rotation;
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
            finalDirection = Vector2.Reflect(finalDirection, other.GetContact(0).normal);
        }
    }
}