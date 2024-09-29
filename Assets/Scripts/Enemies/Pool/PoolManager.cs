using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static PoolManager Instance { get; private set; }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> dictionary;
    private Vector3 safeSpawnPosition = new Vector3(0, 22, 0);
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, safeSpawnPosition, Quaternion.identity);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            dictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnObjectPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!dictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        
        if (dictionary[tag].Count == 0)
        {
            Debug.Log("Pool empty. Creating a new object for tag " + tag);
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                GameObject newObj = Instantiate(pool.prefab, safeSpawnPosition, Quaternion.identity);
                newObj.SetActive(false);
                dictionary[tag].Enqueue(newObj);
            }
        }
        
        if (!IsPositionValid(position))
        {
            Debug.LogWarning("Spawn position invalid, falling back to default.");
            position = GetFallbackPosition();
        }
        
        GameObject spawneable = dictionary[tag].Dequeue();
        spawneable.SetActive(true);
        spawneable.transform.position = safeSpawnPosition;
        spawneable.transform.rotation = rotation;

        return spawneable;
    }

    private bool IsPositionValid(Vector3 position)
    {
        return position.y >= 0;
    }

    private Vector3 GetFallbackPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public void RecycleObject(GameObject obj)
    {
        obj.SetActive(false);
        string tag = obj.GetComponent<Enemy>() != null ? "Enemy" : "Default"; 
        dictionary[tag].Enqueue(obj);
    }
    
    public IEnumerator RecycleAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        RecycleObject(obj);
    }
}