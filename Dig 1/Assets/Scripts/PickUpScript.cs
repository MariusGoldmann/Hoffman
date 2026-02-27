using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class PickUpScript : MonoBehaviour
{
    [SerializeField] GameObject bomerangImage;

    [SerializeField] bool hasLeg;
    [SerializeField] bool hasEye;
    [SerializeField] bool hasEar; 
    [SerializeField] bool hasBoomerang;

    [SerializeField] GameObject eyeTabCloud;
    [SerializeField] GameObject boomerangTabCloud;

    [SerializeField] bool isInteracting;

    InputAction interactAction;

    [SerializeField] GameObject newLegRig;
    [SerializeField] GameObject oldLegRig;
    [SerializeField] GameObject newEarRig;
    [SerializeField] GameObject newEyeRig; 


    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBoomerang = false;
        newLegRig.transform.localScale = new Vector3(0, 0, 0);  
        newEyeRig.transform.localScale = new Vector3(0, 0, 0);
        newEarRig.transform.localScale = new Vector3(0, 0, 0);


        if (bomerangImage != null)
        {
            bomerangImage.SetActive(false);
        }


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
            Animator animator = GetComponentInChildren<Animator>(); 
            animator.SetBool("HasLeg", true);
            animator.SetTrigger("HasLeg");
            newLegRig.transform.localScale = new Vector3(1, 1, 1);
            oldLegRig.transform.localScale = new Vector3(0, 0, 0);

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerEye") && isInteracting == true)  
        {
            hasEye = true;
            newEyeRig.transform.localScale = new Vector3(1, 1, 1);

            eyeTabCloud.SetActive(false);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Boomerang") && isInteracting == true)
        {
            hasBoomerang = true;
            bomerangImage.SetActive(true);
            boomerangTabCloud.SetActive(false);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.CompareTag("PlayerEar") && isInteracting == true)
        {
            hasEar = true;
            newEarRig.transform.localScale = new Vector3(1, 1, 1);
            Destroy(collision.gameObject);
        }
    }

    // Leg Rig 


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
