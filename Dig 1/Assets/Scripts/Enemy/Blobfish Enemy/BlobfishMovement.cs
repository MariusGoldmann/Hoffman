using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BlobfishMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float expandedMoveSpeed = 0.2f;
    [SerializeField] float waypointDistance = 0.1f;
    [SerializeField] Transform[] waypoints;

    [Header("Debug")]
    [SerializeField] int waypointIndex;
    bool movingRight;
    bool facingRight;
    [SerializeField] Vector2 targetPosition;

    Rigidbody2D blobfishRB;
    Animator animator;
    BlobfishCombat blobfishCombat;

    void Start()
    {
        blobfishRB = GetComponent<Rigidbody2D>();
        blobfishCombat = GetComponent<BlobfishCombat>();
    }
    private void FixedUpdate()
    { 
        if (blobfishCombat.GetIsExpanding())
        {
            Move(expandedMoveSpeed);
        }
        else
        {
            Move(moveSpeed);
        }
    }
    void Move(float activeMoveSpeed)
    {
        targetPosition = waypoints[waypointIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, activeMoveSpeed);

        if (waypointIndex == 0 && !movingRight)
        {
            movingRight = true;
        }
        else if (waypointIndex == waypoints.Length - 1 && movingRight)
        {
            movingRight = false;
        }
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
