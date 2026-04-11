using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer interactSprite;
    

    Transform playerTransform;
    PickUpScript pickupScript;


    [SerializeField] bool isWithinInteractDistance;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isWithinInteractDistance)
        {
            Interact();
        }

        if (interactSprite.gameObject.activeSelf && !isWithinInteractDistance)
        {
            interactSprite.gameObject.SetActive(false);
        }
        else if (!interactSprite.gameObject.activeSelf && isWithinInteractDistance)
        {
            interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isWithinInteractDistance = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isWithinInteractDistance = false;
        }
    }
}
