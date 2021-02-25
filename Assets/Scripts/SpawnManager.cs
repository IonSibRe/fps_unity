using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;

    public int spawnEnemyCount = 5;
    public int enemyCount;
    
    public float spawnRange = 10f;
    public float maxDistance = 2.5f;
    public float restrictedSpawnRange;

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

        if (NavMesh.SamplePosition(randomPos + transform.position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            if (Vector3.Distance(player.transform.position, hit.position) > restrictedSpawnRange)
            {
                return hit.position;
            }
        }

        return GenerateRandomSpawnPos();
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateRandomSpawnPos() + new Vector3(0f, 2f, 0f), enemyPrefab.transform.rotation);
        }
    }
}
