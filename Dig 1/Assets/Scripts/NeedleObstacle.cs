using Unity.VisualScripting;
using UnityEngine;

public class NeedleObstacle : MonoBehaviour
{
    [SerializeField] CircleCollider2D needleCollider;

    void Start()
    {
        needleCollider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Needle Hit");
        }
    }
}
