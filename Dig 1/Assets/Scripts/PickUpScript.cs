using UnityEngine;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{

    [SerializeField] Image legImage;
    [SerializeField] Image eyeImage;
    [SerializeField] Image bomerangImage;

    [SerializeField] bool hasLeg;
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
            legImage.IsActive();
            Destroy(collision.gameObject);
        }
        collision.gameObject.CompareTag("PlayerEye");
        {
            hasEye = true; 
            eyeImage.IsActive();
            Destroy(collision.gameObject);
        }
        collision.gameObject.CompareTag("Boomerang");
        {
            hasBoomerang = true; 
            bomerangImage.IsActive();
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
