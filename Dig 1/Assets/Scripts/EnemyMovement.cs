using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 1f;
    [SerializeField] float combatDistance = 0f;
    
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform[] waypoints;

    [SerializeField] float distanceToPlayer;
    int waypointIndex;
    bool inCombat;

    Rigidbody2D rigidBody;
    Animator animator;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        ActivateCombat();
        HandleAnimations();
    }
    private void FixedUpdate()
    {
        if (!inCombat)
        {
            IdleMovement();
        }
        else
        {

        }
    }

    void HandleAnimations()
    {

    }
    void ActivateCombat()
    {
        distanceToPlayer = Vector2.Distance(transform.position,playerTransform.position);

        if (distanceToPlayer<combatDistance)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
        }

    }
    void IdleMovement()
    {
        if (waypointIndex <= waypoints.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, idleMoveSpeed);
        }
        else
        {
            waypointIndex = 0;
        }

        if (transform.position == waypoints[waypointIndex].position)
        {
            waypointIndex++;
        }
    }
    void TrackPlayer()
    {

    }
}
