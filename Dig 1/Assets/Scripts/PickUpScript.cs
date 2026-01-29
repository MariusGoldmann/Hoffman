using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class PickUpScript : MonoBehaviour
{
    [SerializeField] GameObject bomerangImage;

    [SerializeField] bool hasLeg;
    [SerializeField] bool hasEye;
    [SerializeField] bool hasBoomerang;

    [SerializeField] bool isInteracting;

    InputAction interactAction; 

    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBoomerang = false;
        //if (bomerangImage != null)
      
        bomerangImage.SetActive(false);
     

        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Interacted");
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLeg") && isInteracting == true) 
        {
            hasLeg = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerEye") && isInteracting == true)  
        {
            hasEye = true; 
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Boomerang") && isInteracting == true)
        {
            hasBoomerang = true;
            bomerangImage.SetActive(true); 
            Destroy(collision.gameObject);
        }
    }

    public bool GetHasLeg()
    {
        return hasLeg;
    }

    public bool GetHasEye()
    {
        return hasEye;
    }

    public bool GetHasBoomerang()
    {
        return hasBoomerang;
    }

}
