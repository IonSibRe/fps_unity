using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnEnemyCount = 5;
    public int enemyCount;
    public float spawnRange = 150f; 

    void Start()
    {
        SpawnEnemyWave(spawnEnemyCount);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Target>().Length;
        if (enemyCount == 0)
        {
            spawnEnemyCount += 5;
            SpawnEnemyWave(spawnEnemyCount);
        }
    }

    private Vector3 GenerateRandomSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0f, spawnPosZ);

        return randomPos;
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateRandomSpawnPos(), enemyPrefab.transform.rotation);

        }
    }
}
