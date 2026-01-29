using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float idleMoveSpeed = 0.5f;
    [SerializeField] float chaseMoveSpeed = 1f;
    [SerializeField] float waypointDistance = 0.5f;
    
    [SerializeField] Transform[] waypoints;

    float lastFramePositionX;

    [Header ("Debug")]
    [SerializeField] int waypointIndex;
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
        HandleDirection();
        HandleAnimations();
    }

    private void LateUpdate()
    {
        lastFramePositionX = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (state.GetInCombat())
        {
            ChasePlayer();
        }
        else
        {
            IdleMovement();
        }
    }

    public void HandleDirection()
    {
        if (transform.position.x > lastFramePositionX && !facingRight)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x<lastFramePositionX && facingRight)
        {
            facingRight= false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
            rigidBody.MovePosition(Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, idleMoveSpeed));
        }
    }

    void ChasePlayer()
    {
        rigidBody.linearVelocity = new Vector2(state.GetPlayerDirection().x*chaseMoveSpeed, rigidBody.linearVelocityY);
        Debug.Log("chase");
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
