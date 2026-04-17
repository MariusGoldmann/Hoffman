using UnityEngine;
using UnityEngine.InputSystem;

public class LeanTweenTest : MonoBehaviour
{
    [SerializeField] Transform posA;
    [SerializeField] Transform posB;

    void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            MoveTween();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ScaleTween();
        }
    }

    void MoveTween()
    {
        LeanTween.move(gameObject, posA, 1).setEaseInOutSine();
    }

    void ScaleTween()
    {
        LeanTween.scale(gameObject, new Vector2(5, 5), 1).setLoopType(LeanTweenType.easeOutBounce);
    }
}
