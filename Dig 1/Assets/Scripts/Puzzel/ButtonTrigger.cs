using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door;

    int objectsOnButton = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Movable")) return;

        objectsOnButton++;
        door.Open();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Movable")) return;

        objectsOnButton--;

        if (objectsOnButton <= 0)
            door.Close();
    }
}