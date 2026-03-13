using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;
    [SerializeField] Vector3 desiredSize;
    [SerializeField] Vector3 startSize;

    void Awake()
    {
        buttonPressed = false;
    }

    public void Start()
    {
        buttonPressed = false;
        transform.localScale = startSize;
    }

    private void OnEnable()
    {
        transform.localScale = startSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        transform.localScale = startSize;
    }


    private void Update()
    {

        if (buttonPressed == true)
        {
            transform.localScale = desiredSize;
        }
        else
        {
            Debug.Log("Update loop");
            transform.localScale = startSize;
        }

        /* while (buttonPressed && transform.localScale.x < desiredSize.x)
         {
             transform.localScale = Vector3.Lerp(transform.localScale, desiredSize, 10 * Time.deltaTime);
             break;
         }
         while (!buttonPressed)
         {
             transform.localScale = Vector3.Lerp(transform.localScale, desiredSize, 10 * Time.deltaTime);
             break;
         }*/


        /*while (buttonPressed)
     {
        transform.localScale = Vector3.Lerp(transform.localScale, desiredSize, scaleSpeed * Time.deltaTime);
         break;
     }
     while (!buttonPressed)
     {
             transform.localScale = Vector3.Lerp(transform.localScale, startSize, scaleSpeed * Time.deltaTime);
            break;
      }*/
    }



}
