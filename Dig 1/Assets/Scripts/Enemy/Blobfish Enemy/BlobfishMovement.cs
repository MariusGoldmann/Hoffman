using UnityEngine;

public class BlobfishMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float waypointDistance = 0.1f;
    [SerializeField] Transform[] waypoints;

    [Header("Debug")]
    [SerializeField] int waypointIndex;
    [SerializeField] Vector2 targetPosition;

    Rigidbody2D blobfishRB;
    Animator animator;

    void Start()
    {
        blobfishRB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {   
        Move();
    }  

    void Move()
    {
        targetPosition = waypoints[waypointIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);

        if (Vector2.Distance(transform.position, targetPosition) < waypointDistance) waypointIndex++;
        if (waypointIndex==waypoints.Length) waypointIndex = 0;
    }
}
