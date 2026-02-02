using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] ButtonTrigger[] buttons;

    [SerializeField] BoxCollider2D doorCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float openHeight = 3f;
    [SerializeField] float speed = 2f;

    Vector3 startPos;
    bool isOpen = false;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }
    void Update()
    {
        foreach (ButtonTrigger button in buttons)
        {
            if (!button.GetIsPressed())
            {
                return;
            }
        }

        isOpen = true;

        if(isOpen)
        {
            Vector3 targetPos = startPos + Vector3.up * openHeight;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        OpenDoor();
    }



    void OpenDoor()
    {
        doorCollider.enabled = true;
        spriteRenderer.enabled = true;
        transform.Translate(Vector2.up * 2f * Time.deltaTime);
    }
}
