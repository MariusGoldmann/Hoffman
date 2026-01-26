using UnityEngine;

public class PickUpScript : MonoBehaviour
{

    [SerializeField] GameObject leg;
    [SerializeField] GameObject Eye;
    [SerializeField] GameObject Bomerang;

    public bool hasLeg;
    public bool hasEye;
    public bool hasBomerang;

    void Start()
    {
        hasLeg = false;
        hasEye = false;
        hasBomerang = false;
    }

    void Update()
    {

    }

    
}
