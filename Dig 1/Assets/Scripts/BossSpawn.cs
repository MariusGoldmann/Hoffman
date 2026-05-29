using UnityEngine;
using System.Collections;
public class BossSpawn : MonoBehaviour
{
    [SerializeField] GameObject ratPrefab;
    [SerializeField] GameObject dragonPrefab;
    [SerializeField] GameObject pufferFishPrefab;
    [SerializeField] GameObject elephantPrefab;

    int spawnCount;




    BossSpawnStart bossSpawnStartScript;

    void Start()
    {

        bossSpawnStartScript = FindAnyObjectByType<BossSpawnStart>();

    }

    void Update()
    {
        if (bossSpawnStartScript.bossSpawnStart)
        {
            StartCoroutine(SpawnStart());
            Debug.Log("BossFightStart");
        }
    }



    IEnumerator SpawnStart()
    {
        bossSpawnStartScript.canWin = false;
        yield return new WaitForSecondsRealtime(1);
        RatSpawn();
        yield return new WaitForSecondsRealtime(10);

        RatSpawn();
        yield return new WaitForSecondsRealtime(6);
        DragonSpawn();
        StartCoroutine(SecondWave());
    }

    IEnumerator SecondWave()
    {
        yield return new WaitForSecondsRealtime(7);
        Debug.Log("SecondWave1");
        RatSpawn();
        yield return new WaitForSecondsRealtime(4);
        DragonSpawn();
        yield return new WaitForSecondsRealtime(12);

        Debug.Log("SecondWave2");
        RatSpawn();
        yield return new WaitForSecondsRealtime(4);
        RatSpawn();
        yield return new WaitForSecondsRealtime(8);
        DragonSpawn();
        yield return new WaitForSecondsRealtime(5);
        ElephantSpawn();
        bossSpawnStartScript.canWin = true;
    }

    //void RandomSpawn()
    //{
    //    spawnCount = UnityEngine.Random.Range(1, 10);

    //    // this have to change if we want to add the elephant or Pufferfish too. 
    //    if (spawnCount <= 6)
    //    {
    //        RatSpawn();
    //    }
    //    //else if (spawnCount >= 4 && spawnCount <= 6)
    //    //{
    //    //    PufferFishSpawn();
    //    //}
    //    else if (spawnCount >= 7)
    //    {
    //        DragonSpawn();
    //    }
    //}


    void RatSpawn()
    {
        Debug.Log("Rat");

        // Vector3 pos1 = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(0f, 0f), 0f);
        Instantiate(ratPrefab, transform.position, Quaternion.identity);

        bossSpawnStartScript.enemyCountBoss++;
    }

    void DragonSpawn()
    {
        Debug.Log("Dragon");

        // Vector3 pos2 = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(0f, 0f), 0f);
        Instantiate(dragonPrefab, transform.position, Quaternion.identity);

        bossSpawnStartScript.enemyCountBoss++;

    }

    //void PufferFishSpawn()
    //{
    //    Debug.Log("PufferFish");

    //    Instantiate(pufferFishPrefab, transform.position, Quaternion.identity);
    //    bossSpawnStartScript.enemyCountBoss++;

    //}

    void ElephantSpawn()
    {
        Debug.Log("Elephant");

        Instantiate(elephantPrefab, transform.position, Quaternion.identity);
        bossSpawnStartScript.enemyCountBoss++;

    }




}