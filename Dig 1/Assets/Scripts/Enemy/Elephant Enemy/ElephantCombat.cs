using System.Collections;
using UnityEngine;
public class ElephantCombat : MonoBehaviour
{
    [Header("Trumpet Attack")]
    [SerializeField] bool trumpetAttack;
    [SerializeField] float projectileAmount = 3;
    [SerializeField] float trumpetAnticipationTime = 1;
    [SerializeField] float trumpetShockwaveSpeed = 5f;
    [SerializeField] Vector2 trumpetLocation;
    [SerializeField] Vector2 trumpetDirection;

    [Header("Stomp attack")]
    [SerializeField] bool stompAttack;
    [SerializeField] float stompAnticipationTime = 1;

    [Header("References")]
    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ObjectPooling stompPool;
    [SerializeField] ElephantMovement elephantMovement;

    private void Update()
    {
        if (trumpetAttack)
        {
            StartCoroutine(TrumpetAttack());
            trumpetAttack = false;
        }
        if (stompAttack)
        {
            StartCoroutine(StompAttack());
            stompAttack = false;
        }
    }
    IEnumerator TrumpetAttack()
    {
        float elapsedTime = 0;

        while (elapsedTime > trumpetAnticipationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Vector2 initialDirection = new Vector2;
        
        for (int i; i<projectileAmount; i++))
        {
            GameObject[i] projectile = trumpetPool.GetObject(transform.position, Quaternion.identity); //Change transform.position to trumpetPosition
        }
        
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        TrumpetProjectile projectileScript = projectile.GetComponent<TrumpetProjectile>();
        yield return new WaitForSeconds(2);
    }
    IEnumerator StompAttack()
    {
        float elapsedTime = 0;

        while (elapsedTime > stompAnticipationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public Vector2 GetInitialDirection()
    {
        return new Vector2(trumpetDirection.x * elephantMovement.GetFacingDirection(), trumpetDirection.y);
    }
}