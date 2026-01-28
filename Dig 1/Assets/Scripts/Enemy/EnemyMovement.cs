using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 1f;
    [SerializeField] float waypointDistance = 0.2f;
    
    [SerializeField] Transform[] waypoints;

    [Header ("Debug")]
    [SerializeField] int waypointIndex;

    Rigidbody2D rigidBody;
    Animator animator;
    EnemyState state;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        state = GetComponent <EnemyState>();
    }

    void Update()
    {
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        if (!state.GetInCombat())
        {
            IdleMovement();
        }
        else
        {
            ChasePlayer();
        }
    }

    void HandleAnimations()
    {

    }

    void IdleMovement()
    {
        if (waypointIndex==waypoints.Length)
        {
            waypointIndex = 0;
        }
        else if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < waypointDistance)
        {
            waypointIndex++;
        }
        else 
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, idleMoveSpeed);
        }
    }

    void ChasePlayer()
    {

    }
}
