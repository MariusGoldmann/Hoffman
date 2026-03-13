using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float zoomSize = 2.10f;
    [SerializeField, Range(0.01f, 1f)] float zoomSpeed;
  
    [SerializeField] Vector3 zoomPosition = new Vector3(0,0, -10);
    [SerializeField] float minZoom = 2.1f;   
    [SerializeField] float maxZoom = 6f;     

    bool isZoomingIn;


    Camera cam;
    Rigidbody2D playerRb;
    Transform playerTransform;

    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isZoomedOut = !isZoomedOut;
        }
    }

    private void LateUpdate()
    {
        if(isZoomedOut)
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
        float targetZoom = Mathf.Lerp(cam.orthographicSize, maxZoom, zoomSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    void ZoomOut()
    {
        float targetZoom = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

}
