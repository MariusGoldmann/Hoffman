using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;
    [SerializeField] float smoothing = 0.6f;

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetToFollow.transform.position.x,smoothing),
            Mathf.Lerp(transform.position.y,targetToFollow.transform.position.y,smoothing),
            transform.position.z);

    }
}