using System;
using UnityEngine;

public class TrumpetProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timeUntilDeath;

    Vector2 direction;
    Vector2 wallDirection;

    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ElephantCombat elephantCombat;

    private void Start()
    {
        direction = new Vector2(1, 1);
    }

    private void Update()
    {
        {
            Vector2 finalDirection =  elephantCombat.GetInitialDirection() * direction;
            projectileRB.linearVelocity = finalDirection * trumpetShockwaveSpeed;
            projectileRB.transform.rotation = Quaternion.LookRotation(finalDirection);
            //projectileRB.transform.rotation = Quaternion.Euler(0, 0, -MathF.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg);
            Debug.Log(finalDirection);
            yield return null;
        }
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

    public Vector2 GetDirection()
    {
        return direction;
    }
}
