using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BossSpawnStart : MonoBehaviour
{
    public bool bossSpawnStart;


    [SerializeField] public int enemyCountBoss;
    public bool win;
    public bool canWin;
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
       

        if (canWin == true && enemyCountBoss == 0)
        {
            win = true;
            Debug.Log("BossFightWin");  
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && bossSpawnStart == false)
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


    public bool GetCanWin()
    {
        return canWin;
    }

}
