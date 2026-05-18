using UnityEngine;

public class BossSpawnStart : MonoBehaviour
{
    public bool bossSpawnStart;


    [SerializeField] public int enemyCountBoss;
    public bool win;
    bool canWin;

    void Start()
    {
        win = false;
    }
    void Awake()
    {
        canWin = false; 

        bossSpawnStart = false;

        enemyCountBoss = 0;
    }

    void Update()
    {
        if (enemyCountBoss >= 1)
        {
           canWin = true;
        }

        if (enemyCountBoss == 0 && canWin == true)
        {
            win = true;
        }
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

    public bool GetIsBossFightWin()    // This bool is true when the whole game is done, the player won. 
    {
        return win;
    }

    public int GetEnemyCountBoss()
    {
        return enemyCountBoss;
    }


}
