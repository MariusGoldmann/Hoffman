using System;
using System.Collections;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
public class ElephantCombat : MonoBehaviour
{
    [Header("Trunk Attack")]
    [SerializeField] bool trumpetAttack;
    [SerializeField] float trumpetAnticipationTime = 1;
    [SerializeField] float trumpetShockwaveSpeed = 5f;
    [SerializeField] Vector2 trumpetLocation;
    [SerializeField] Vector2 trumpetDirection;

    [Header("Stomp")]
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
        float elapsedTime= 0;

        while (elapsedTime > trumpetAnticipationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Quaternion projectileQuaternion = Quaternion.Euler(0, 0, MathF.Atan2(trumpetDirection.y, trumpetDirection.x) * Mathf.Rad2Deg);
        Vector2 initialDirection = new Vector2(trumpetDirection.x * elephantMovement.GetFacingDirection(), trumpetDirection.y);
        GameObject projectile = trumpetPool.GetObject(transform.position, projectileQuaternion); //Change transform.position to trumpetPosition
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        TrumpetProjectile projectileScript = projectile.GetComponent<TrumpetProjectile>();
        while (projectile != null)
        {
            projectileRB.linearVelocity =  projectileScript.GetDirection() * initialDirection * trumpetShockwaveSpeed;
            yield return null;
        }
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

}