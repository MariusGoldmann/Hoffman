using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer interactSprite;
    

    Transform playerTransform;
    PickUpScript pickupScript;
    

    float interactDistance = 5f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && IsWithinInteractDistance())
        {
            Interact();
        }

        if (interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            interactSprite.gameObject.SetActive(false);
        }
        else if (!interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact();

    bool IsWithinInteractDistance()
    {
        if (Vector2.Distance(playerTransform.position, gameObject.transform.position) < interactDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
