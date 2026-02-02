using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 2f, 0);
    public float moveSpeed = 5f;

    Vector3 closedPos;
    Vector3 targetPos;

    void Start()
    {
        closedPos = transform.position;
        targetPos = closedPos;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    public void Open()
    {
        targetPos = closedPos + openOffset;
    }

    public void Close()
    {
        targetPos = closedPos;
    }
}