using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] Light2D light2D;

    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;
    [SerializeField] float lightChangeSpeed;


    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        float lerpValue = Mathf.PingPong(Time.time * lightChangeSpeed, 1f);

        light2D.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, lerpValue);
    }
}
