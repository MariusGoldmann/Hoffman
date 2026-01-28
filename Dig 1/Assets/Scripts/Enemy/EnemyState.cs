using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] float combatDistance=0.2f;
    [SerializeField] Transform playerTransform;

    bool inCombat;

    private void Update()
    {
        CheckForCombat();
    }

    void CheckForCombat()
    {
        float distanceToPlayer;
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < combatDistance)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
        }

    }

    public bool GetInCombat()
    {
        return inCombat; 
    }
}
