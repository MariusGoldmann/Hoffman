using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    [Header("Pick Up Bools")]
    [SerializeField] bool hasLeg;
    [SerializeField] bool hasEye;
    [SerializeField] bool hasEar;
    [SerializeField] bool hasBoomerang;
    [SerializeField] bool isInteracting;

    InputAction interactAction;

    [Header("UI")]
    [SerializeField] GameObject eyeTabCloud;
    [SerializeField] GameObject boomerangTabCloud;

    [Header("Particles")]
    [SerializeField] ParticleSystem pickUpLegParticle;
    [SerializeField] ParticleSystem pickUpEyeParticle;
    [SerializeField] ParticleSystem pickUpEarParticle;


    [Header("Rigs")]
    [SerializeField] GameObject newLegRig;
    [SerializeField] GameObject oldLegRig;
    [SerializeField] GameObject newEarRig;
    [SerializeField] GameObject newEyeRig;

    [SerializeField] Animator animator;

    SpawnManager spawnManager;

    void Awake()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }
    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBoomerang = false;
        hasEar = false;

        newEyeRig.transform.localScale = new Vector3(0, 0, 0);
        newEarRig.transform.localScale = new Vector3(0, 0, 0);

        animator = GetComponentInChildren<Animator>();

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

    private void Update()
    {
        if (hasLeg == true)
        {
            animator.SetBool("HasLeg", true);
            Debug.Log("Has leg");
            newLegRig.transform.localScale = new Vector3(1, 1, 1);
            oldLegRig.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            animator.SetBool("HasLeg", false);
            Debug.Log("No leg");
            newLegRig.transform.localScale = new Vector3(0, 0, 0);
            oldLegRig.transform.localScale = new Vector3(1, 1, 1);
        }
        RigSetter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLeg") && isInteracting == true)
        {
            hasLeg = true;
            spawnManager.legOwned = true;


            pickUpLegParticle.Play();

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerEye") && isInteracting == true)
        {
            hasEye = true;
            spawnManager.eyeOwned = true;
            pickUpEyeParticle.Play();
            newEyeRig.transform.localScale = new Vector3(1, 1, 1);

            eyeTabCloud.SetActive(false);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Boomerang") && isInteracting == true)
        {
            hasBoomerang = true;
            spawnManager.boomerangOwned = true;

            boomerangTabCloud.SetActive(false);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("PlayerEar") && isInteracting == true)
        {
            hasEar = true;
            spawnManager.earOwned = true;
            pickUpEarParticle.Play();
            newEarRig.transform.localScale = new Vector3(1, 1, 1);
            Destroy(collision.gameObject);
        }
    }

    void RigSetter()
    {
        if (spawnManager.legOwned == true)
        {
            hasLeg = true;
        }

        if (spawnManager.eyeOwned == true)
        {
            hasEye = true;
            newEyeRig.transform.localScale = new Vector3(1, 1, 1);

            eyeTabCloud.SetActive(false);
        }

        if (spawnManager.boomerangOwned == true)
        {
            hasBoomerang = true;

            boomerangTabCloud.SetActive(false);
        }

        if (spawnManager.earOwned == true)
        {
            hasEar = true;

            newEarRig.transform.localScale = new Vector3(1, 1, 1);
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

    public bool GetHasEar()
    {
        return hasEar;
    }

}
