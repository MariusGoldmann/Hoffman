using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int playerHealth = 10;

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
