using System.Collections;
using UnityEngine;

public class KnockbackScript : MonoBehaviour
{
    [SerializeField] float knockbackLength=0.1f;

    bool isKnockback=false;

    Rigidbody2D knockbackRigidbody;
    PlayerMovement playerMovement;

    private void Start()
    {
        knockbackRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 additionalForceDirection, float hitForce, float additionalForce)
    {
        isKnockback = true;
        
        float elapsedTime = 0f;
        Vector2 knockbackForce;
        Vector2 combinedForce;

        if (hitDirection.x > 0) hitDirection = Vector2.right;
        else hitDirection = Vector2.left;

        knockbackForce = hitDirection * hitForce + additionalForceDirection * additionalForce;

        while (elapsedTime < knockbackLength)
        {
            elapsedTime += Time.fixedDeltaTime;

            if (playerMovement != null) combinedForce = knockbackForce + playerMovement.GetMoveInput();
            else combinedForce = knockbackForce;

            knockbackRigidbody.linearVelocity = combinedForce;

            yield return new WaitForFixedUpdate();
        }

        isKnockback=false;
    }

    public bool GetIsKnockback()
    {
        return isKnockback; 
    }
}
