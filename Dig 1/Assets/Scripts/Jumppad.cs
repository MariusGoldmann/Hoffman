using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField] float jumppadForce;
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 0;
            Debug.Log("Jumppad");
            animator.SetTrigger("Contact");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumppadForce, ForceMode2D.Impulse);  
        }
    }
}
