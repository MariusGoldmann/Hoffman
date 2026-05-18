using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class ElephantCombat : MonoBehaviour
{
    [SerializeField] float attackCooldown = 1f;

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

    bool isAlive = true;
    float currentCooldown;

    [Header("References")]
    [SerializeField] ObjectPooling trumpetPool;
    [SerializeField] ObjectPooling stompPool;
    [SerializeField] ElephantMovement elephantMovement;
    [SerializeField] KnockbackScript knockbackScript;
    [SerializeField] EnemyHealth enemyHealth;

    public Coroutine trumpetCoroutine;
    public Coroutine stompCoroutine;

    private void Update()
    {
        if (CanAttack())
        {
            currentCooldown = attackCooldown;
            float attackPicker = Random.Range(0, 2);
            if (attackPicker==0) trumpetCoroutine = StartCoroutine(TrumpetAttack());
            else stompCoroutine=StartCoroutine(StompAttack());
        }
        currentCooldown -= Time.deltaTime;
    }
    bool CanAttack()
    {
        if (isAlive && currentCooldown < 0 && !knockbackScript.GetIsKnockback() && elephantMovement.GetPlayerTarget() != null)
        {
            return true;
        }
        return false;
    }
    public IEnumerator TrumpetAttack()
    {
        for (int i=0; i<projectileAmount; i++)
        {
            trumpetPool.GetObject(transform.position, Quaternion.identity); //Change transform.position to trumpetPosition
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }

        trumpetCoroutine = null;
    }
    public IEnumerator StompAttack()
    {
        Debug.Log("Did stomp attack");
        yield return null;
    }
    public Vector2 GetInitialDirection()
    {
        return new Vector2(trumpetDirection.x * elephantMovement.GetFacingDirection(), trumpetDirection.y);
    }
}