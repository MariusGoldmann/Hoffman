using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraDirection : MonoBehaviour
{
    [SerializeField] float directionChangeSpeed;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineFramingTransposer framingTransposer;

    PlayerMovement playerMovement;

    void Awake()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        StartCoroutine(FacingDirection());
    }

    IEnumerator FacingDirection()
    {
        float lerpSpeed = directionChangeSpeed;
        while (true)
        {
            float targetPosition = DetermineEndPoint();
            framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, targetPosition, lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    float DetermineEndPoint()
    {
        if (playerMovement.GetFacingDirection() > 0)
        {
            return 0.45f;
        }
        else
        {
            return 0.55f;
        }
    }
}
