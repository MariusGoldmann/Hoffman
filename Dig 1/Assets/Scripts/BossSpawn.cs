using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using Unity.Collections;
using System;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] GameObject ratPrefab;
    [SerializeField] GameObject dragonPrefab;
    [SerializeField] GameObject pufferFishPrefab;
   // [SerializeField] GameObject elephantPrefab;

    int spawnCount;

    BossSpawnStart bossSpawnStartScript;

    void Start()
    {

        Collider2D collider = GetComponentInParent<Collider2D>();

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
        bossSpawnStartScript.bossSpawnStart = false;
        yield return new WaitForSecondsRealtime(1);
        RandomSpawn();
        yield return new WaitForSecondsRealtime(5);

        RandomSpawn();
        yield return new WaitForSecondsRealtime(1);
        RandomSpawn();
    }

    void RandomSpawn()
    {
        spawnCount = UnityEngine.Random.Range(1, 10);

        // this have to change if we want to add the elephant too. 
        if (spawnCount <= 4)
        {
            RatSpawn();
        }
        else if (spawnCount >= 5 && spawnCount <= 7)
        {
            PufferFishSpawn();
        }
        else if (spawnCount >= 8 && spawnCount == 10)
        {
            DragonSpawn();
        }
    }


    void RatSpawn()
    {
        Debug.Log("Rat");

        Instantiate(ratPrefab, transform.position, Quaternion.identity);
    }

    void DragonSpawn()
    {
        Debug.Log("Dragon");

        Instantiate(dragonPrefab, transform.position, Quaternion.identity);
    }

    void PufferFishSpawn()
    {
        Debug.Log("PufferFish");

        Instantiate(pufferFishPrefab, transform.position, Quaternion.identity);
    }

    /*void ElephantSpawn()
    {
        Debug.Log("Elephant");

        Instantiate(elephantPrefab, transform.position, Quaternion.identity);
    } */
}