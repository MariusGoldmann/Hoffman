using UnityEngine;

public class TrumpetProjectile : MonoBehaviour
{
    [SerializeField] ObjectPooling trumpetPool;
    Vector2 direction;

    public Vector2 GetDirection()
    {
        return direction;
    }
}
