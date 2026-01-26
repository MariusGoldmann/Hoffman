using UnityEngine;
using UnityEngine.UIElements;

public class PickUpScript : MonoBehaviour
{

    [SerializeField] Image legImage;
    [SerializeField] Image eyeImage;
    [SerializeField] Image bomerangImage;


    bool hasLeg;
    bool hasEye;
    bool hasBoomerang;

    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBoomerang = false;
    }

    void Update()
    {
          
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.CompareTag("PlayerLeg");
        {
            hasLeg = true; 
            Destroy(collision.gameObject);
        }
        collision.gameObject.CompareTag("PlayerEye");
        {
            hasEye = true; 
            Destroy(collision.gameObject);
        }
        collision.gameObject.CompareTag("Boomerang");
        {
            hasBoomerang = true; 
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
