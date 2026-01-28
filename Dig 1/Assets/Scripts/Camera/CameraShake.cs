using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Message;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 originalPosition;
    [SerializeField] bool isShaking = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            ShakeCamera(0.5f, 0.05f, true, true);
        }
    }

    private void ShakeCamera(float duration, float severity, bool vertical, bool horizontal)
    {
        if (isShaking)
        {
            return;
        }

        StartCoroutine(Shake(duration, severity, vertical, horizontal));
    }

    private IEnumerator Shake(float duration, float severity, bool vertical, bool horizontal)
    {
        originalPosition = transform.localPosition;

        isShaking = true;

        while( duration > 0)
    {
            Vector3 shakeOffset = Vector3.zero;

            if (horizontal)
            {
                shakeOffset.x = Random.Range(-1f, 1f) * severity;
            }
            if (vertical)
            {
                shakeOffset.y = Random.Range(-1f, 1f) * severity;
            }

            transform.localPosition = originalPosition + shakeOffset;

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;

        isShaking = false;

    }

    
}