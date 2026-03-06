using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class RotateDoor : MonoBehaviour
{
    [SerializeField] float RotatenTime = 0.3f;
    [SerializeField] bool IsVertical = false;

    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            SpinningDoor();
        }
    }
    void SpinningDoor()
    {
        float targetRotation = IsVertical ? 0f : 90f;

        StopAllCoroutines();
        StartCoroutine(RotateSmooth(targetRotation));

        IsVertical = !IsVertical;
    }
    IEnumerator RotateSmooth(float targetZ)
    {
        float time = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZ);

        while (time < RotatenTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / RotatenTime);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
