using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] bool isActive;

    [SerializeField] Light2D innerSpotLight;
    [SerializeField] Light2D outerSpotLight;
    [SerializeField] Animator animator;

    SpawnManager spawnManager;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spawnManager = FindAnyObjectByType<SpawnManager>();
    }
    void Update()
    {
        if (isActive)
        {
            innerSpotLight.enabled = true;
            outerSpotLight.enabled = true;
        }
        else
        {
            innerSpotLight.enabled = false;
            outerSpotLight.enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActive)
        {
            animator.SetTrigger("Activated");
            
            PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.BELL, 1f);
            isActive = true;
            spawnManager.spawnPosition = transform.position;
        }
    }
}
