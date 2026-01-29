using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float chaseMoveSpeed = 1f;
    [SerializeField] float waypointDistance = 0.5f;
    
    [SerializeField] Transform[] waypoints;

    [Header ("Debug")]
    [SerializeField] int waypointIndex;
    bool inRange;
    bool facingRight;

    Rigidbody2D rigidBody;
    Animator animator;
    EnemyState state;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        state = GetComponentInChildren<EnemyState>();
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
            facingRight = !facingRight;
        }
        else 
        {
            rigidBody.MovePosition(Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, idleMoveSpeed));
        }
    }

    void ChasePlayer()
    {
        rigidBody.linearVelocity = new Vector2(state.GetPlayerDirection().x*chaseMoveSpeed, rigidBody.linearVelocityY);
        Debug.Log("chase");
    }

    public void TurnAround()
    {

    }

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
