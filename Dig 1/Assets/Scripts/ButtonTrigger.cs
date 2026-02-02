using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    [SerializeField] bool isPressed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Movable"))
        {
            isPressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Movable"))
        {
            isPressed = false;
        }
    }

    public bool GetIsPressed()
    {
        return isPressed;
    }
}
