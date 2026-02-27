using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BlobfishMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float waypointDistance = 0.1f;
    [SerializeField] Transform[] waypoints;

    [Header("Debug")]
    [SerializeField] int waypointIndex;
    bool movingRight;
    bool facingRight;
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

        if (waypointIndex == 0 && !movingRight)
        {
            movingRight = true;
        }
        else if (waypointIndex == waypoints.Length - 1 && movingRight)
        {
            movingRight = false;
        }
        Debug.Log(waypointIndex);
        Debug.Log(movingRight);
        if (Vector2.Distance(transform.position, targetPosition) < waypointDistance)
        {
            if (movingRight)
            {
                waypointIndex++;
                facingRight = true;
            }
            else
            {
                waypointIndex--;
                facingRight = false;
            }
        }
    }
    public bool GetFacingRight()
    {
        return facingRight;
    }
}
