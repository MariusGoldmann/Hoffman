using System.Collections;
using UnityEngine;
public class ElephantCombat : MonoBehaviour
{
    [Header("Trumpet Attack")]
    [SerializeField] bool trumpetAttack;
    [SerializeField] float trumpetAnticipationTime = 1;
    [SerializeField] float projectileAmount = 3;
    [SerializeField] float timeBetweenProjectiles = 0.2f;
    [SerializeField] Vector2 trumpetLocation;
    [SerializeField] Vector2 trumpetDirection;

    [Header("Stomp attack")]
    [SerializeField] bool stompAttack;
    [SerializeField] float stompAnticipationTime = 1;

    [Header("References")]
    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ObjectPooling stompPool;
    [SerializeField] ElephantMovement elephantMovement;

    public Coroutine trumpetCoroutine;
    public Coroutine stompCoroutine;

    private void Update()
    {
        if (trumpetAttack)
        {
            trumpetCoroutine=StartCoroutine(TrumpetAttack());
            trumpetAttack = false;
        }
        if (stompAttack)
        {
            stompCoroutine=StartCoroutine(StompAttack());
            stompAttack = false;
        }
    }
    public IEnumerator TrumpetAttack()
    {
        float elapsedTime = 0;

        yield return new WaitForSeconds(trumpetAnticipationTime);
        
        for (int i=0; i<projectileAmount; i++)
        {
            trumpetPool.GetObject(transform.position, Quaternion.identity); //Change transform.position to trumpetPosition
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }

        trumpetCoroutine = null;
    }
    public IEnumerator StompAttack()
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