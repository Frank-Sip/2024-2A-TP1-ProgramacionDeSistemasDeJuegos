using System;
using System.Collections;
using UnityEngine;
using Enemies;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int spawnsPerPeriod = 10;
    [SerializeField] private float frequency = 30;
    [SerializeField] private float period = 0;

    private PoolManager poolManager;

    private void OnEnable()
    {
        if (frequency > 0) period = 1 / frequency;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        poolManager = PoolManager.Instance;

        while (true) 
        {
            for (int i = 0; i < spawnsPerPeriod; i++)
            {
                Vector3 spawnPosition = transform.position;
                GameObject enemyObj = poolManager.SpawnObjectPool("Enemy", spawnPosition, Quaternion.identity);

                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetPoolManager(poolManager);
                    enemy.SpawnObject(spawnPosition);
                }
            }

            yield return new WaitForSeconds(period);
        }
    }
}