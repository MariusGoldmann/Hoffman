using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;
    [SerializeField] float smoothing = 0.6f;
    [SerializeField] Vector3 offset = Vector3.zero;

    Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;

    void Start()
    {
        FaindTargetPosition();
        transform.position = targetPosition;
    }
    void Update()
    {
        FaindTargetPosition();

        transform.position = Vector3.SmoothDamp(
           transform.position,
           targetPosition,
           ref velocity,
           smoothing);
    }
    void FaindTargetPosition()
    {
        targetPosition = new Vector3(
           targetToFollow.position.x,
           targetToFollow.position.y,
           transform.position.z) + offset;
    }
}
