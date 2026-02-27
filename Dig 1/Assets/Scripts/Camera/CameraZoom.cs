using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float zoomSize = 2.10f;
    [SerializeField, Range(0.01f, 10f)] float zoomSpeed = 4f;

    [SerializeField] Vector3 zoomPosition = new Vector3(0, 0, -10);
    [SerializeField] float minZoom = 2.1f;
    [SerializeField] float maxZoom = 6f;

    bool playerIsMoving;
    bool isZoomedOut;

    [SerializeField] CinemachineVirtualCamera vcam;
    Camera cam;
    Rigidbody2D playerRb;
    Transform playerTransform;

    PickUpScript pickUpScript;

    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        pickUpScript = playerTransform.GetComponent<PickUpScript>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        playerIsMoving = playerRb.linearVelocity.sqrMagnitude > 0.01f;

        if (Keyboard.current.zKey.wasPressedThisFrame && !playerIsMoving && pickUpScript.GetHasEye())
        {
            isZoomedOut = !isZoomedOut;
        }

        if (playerIsMoving)
        {
            isZoomedOut = false;
        }
    }

    private void LateUpdate()
    {
        if (isZoomedOut)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }
    void ZoomIn()
    {
        float targetZoom = Mathf.Lerp(vcam.m_Lens.OrthographicSize, maxZoom, zoomSpeed * Time.deltaTime);
        vcam.m_Lens.OrthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    void ZoomOut()
    {
        float targetZoom = Mathf.Lerp(vcam.m_Lens.OrthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
        vcam.m_Lens.OrthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }
}