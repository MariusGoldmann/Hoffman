using UnityEngine;

public class BossSpawnStart : MonoBehaviour
{
    public bool bossSpawnStart;

    void Awake()
    {
        bossSpawnStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ContactWithPlayer");

            bossSpawnStart = true;
        }
    }

    public bool GetIsBossFightStart()
    {
        return bossSpawnStart;
    }

}
