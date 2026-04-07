using UnityEngine;

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

    BlobfishCombat blobfishCombat;

    void Start()
    {
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
        if (facingRight) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    void Move(float activeMoveSpeed)
    {
        if (waypoints.Length > 0)
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
    }
    public bool GetFacingRight()
    {
        return facingRight;
    }
}
