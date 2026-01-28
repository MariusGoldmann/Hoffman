using UnityEngine;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] GameObject bomerangImage;

    [SerializeField] bool hasLeg;
    [SerializeField] bool hasEye;
    [SerializeField] bool hasBoomerang;

    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBoomerang = false;
        bomerangImage.SetActive(false);
    }

    void Update()
    {
          
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLeg"))
        {
            hasLeg = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerEye"))
        {
            hasEye = true; 
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Boomerang"))
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
