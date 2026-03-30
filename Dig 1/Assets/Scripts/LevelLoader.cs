using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
}
