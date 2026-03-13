using Cinemachine;
using UnityEngine;

public class Bob : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
   
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

   public void damage(float damageAmount)
   {
        CameraShakeManager.instance.CameraShake(impulseSource);
   }
}
