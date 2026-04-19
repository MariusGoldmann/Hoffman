using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactSprite;
    

    Transform playerTransform;
    PickUpScript pickupScript;


    [SerializeField] bool isWithinInteractDistance;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isWithinInteractDistance || Keyboard.current.enterKey.wasPressedThisFrame && isWithinInteractDistance)
        {
            Interact();
        }

        if (!isWithinInteractDistance)
        {
            if (!interactSprite.LeanIsTweening())
            {
                LeanTween.scale(interactSprite, new Vector2(0, 0), 0.5f).setEaseInExpo();
            }
        }
        else
        {
            if (!interactSprite.LeanIsTweening())
            {
                LeanTween.scale(interactSprite, new Vector2(0.25f, 0.25f), 1).setEaseOutExpo();
            }
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
