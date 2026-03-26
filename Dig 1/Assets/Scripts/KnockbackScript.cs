using System.Collections;
using UnityEngine;

public class KnockbackScript : MonoBehaviour
{
    [SerializeField] float knockbackLength=0.2f;
    [SerializeField] bool debugBool;

    bool isKnockback=false;

    Rigidbody2D knockbackRigidbody;
    PlayerMovement playerMovement;

    private void Start()
    {
        knockbackRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 additionalForceDirection, float hitDirectionForce, float additionalDirectionalForce)
    {
        isKnockback = true;
        
        float time = 0f;
        Vector2 hitForce;
        Vector2 additionalForce;
        Vector2 combinedForce;

        hitForce = hitDirection * hitDirectionForce;
        additionalForce = additionalForceDirection * additionalDirectionalForce;


        while (time < knockbackLength)
        {
            time += Time.fixedDeltaTime;
            combinedForce = hitForce + additionalForce + playerMovement.GetMoveInput();

            yield return new WaitForFixedUpdate();

            knockbackRigidbody.linearVelocity = combinedForce;
        }

        isKnockback=false;
    }

    public bool GetIsKnockback()
    {
        return isKnockback; 
    }
}
