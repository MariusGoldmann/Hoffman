using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void FadeOut()
    {
        animator.SetTrigger("Start");
    }
}
