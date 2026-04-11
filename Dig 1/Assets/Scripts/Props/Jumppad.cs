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
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.PUMPKIN, 1);
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 0;
            animator.SetTrigger("Contact");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumppadForce, ForceMode2D.Impulse);  
        }
    }
}
