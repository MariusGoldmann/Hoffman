using Unity.VisualScripting;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] Transform button;
    [SerializeField] float pullSpeed=100f;

    float buttonDistance;
    float closeEnoughDistance=1f;

    void Update()
    {
        FindTarget();
    }

    void FindTarget()
    {
        buttonDistance= Vector2.Distance(button.position, transform.position);

        if (buttonDistance < closeEnoughDistance )
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        Vector2.MoveTowards(transform.position, button.position, pullSpeed);
    }

}
