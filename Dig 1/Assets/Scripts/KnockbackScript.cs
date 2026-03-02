using System.Collections;
using UnityEngine;

public class KnockbackScript : MonoBehaviour
{
    [SerializeField] float knockbackLength=0.2f;
    [SerializeField] float hitDirectionForce=10f;
    [SerializeField] float additionalDirectionalForce=5f;
    [SerializeField] float someOtherForce=7.5f;

    bool isKnockback;

    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 additionalForceDirection, float inputDirection)
    {
        isKnockback = true;
        
        float time = 0f;
        Vector2 hitForce;
        Vector2 additionalForce;
        Vector2 combinedForce;

        hitForce = hitDirection * hitDirectionForce;
        additionalForce = additionalForceDirection * additionalDirectionalForce;


        while (time > knockbackLength)
        {
            time += Time.fixedDeltaTime;
            combinedForce = hitForce + additionalForce + new Vector2(inputDirection, 0);

            yield return new WaitForFixedUpdate();

            rigidbody.linearVelocity = combinedForce;
        }

        isKnockback=false;
    }

    public bool GetIsKnockback()
    {
        return isKnockback; 
    }
}
