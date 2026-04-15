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

    [SerializeField] GameObject trumpetShockwave;
    [SerializeField] GameObject stompShockwave;

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
        GameObject projectile = Instantiate(trumpetShockwave, transform.position, projectileQuaternion); //Change transform.position to trumpetPosition
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        while (projectile != null)
        {
            projectileRB.linearVelocity = trumpetDirection * trumpetShockwaveSpeed;
            yield return null;
        }
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