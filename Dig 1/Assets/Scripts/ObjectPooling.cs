using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class ObjectPooling : MonoBehaviour
{
    public GameObject prefab;
    Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetObject(Vector2 spawnPosition, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            obj.transform.position=spawnPosition;
            obj.transform.rotation=rotation;
            return obj;
        }
        return Instantiate(prefab, spawnPosition, rotation);
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
