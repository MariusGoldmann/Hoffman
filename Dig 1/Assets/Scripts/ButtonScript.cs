using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class ButtonScript : MonoBehaviour
{
    private Button buttonElement;

    //void Awake()
    //{
    //    buttonPressed = false;
    //}

    public void Start()
    {
        buttonElement = GetComponent<Button>();
        buttonElement.onClick.AddListener(OnButtonPressed);
    }

    //private void OnEnable()
    //{
    //    transform.localScale = startSize;
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    buttonPressed = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    buttonPressed = false;
    //    transform.localScale = startSize;
    //}

    public void OnButtonPressed()
    {
        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BUTTON, 1);
    }

    


    //private void Update()
    //{

    //    if (buttonPressed == true)
    //    {
    //        transform.localScale = desiredSize;
    //        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BUTTON, 1);
    //    }
    //    else
    //    {
    //        Debug.Log("Update loop");
    //        transform.localScale = startSize;
    //    }

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
    //}



}
