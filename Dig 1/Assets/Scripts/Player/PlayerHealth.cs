using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int playerHealth;

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
