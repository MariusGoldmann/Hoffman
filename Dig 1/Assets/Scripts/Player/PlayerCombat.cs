using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] float meleeAttackTime = 0.2f;

    [SerializeField]  Collider2D meleeCollider;
    private void Awake()
    {
        meleeCollider.enabled = false;
    }

    void OnMelee(InputValue value)
    {
        if (value.isPressed)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    IEnumerator MeleeAttack()
    {
        meleeCollider.enabled = true;

        yield return new WaitForSeconds(meleeAttackTime);

        meleeCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
