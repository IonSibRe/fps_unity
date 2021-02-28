using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject pressureTank;
    public GameObject player;
    public GameObject center;

    public int spawnEnemyCount = 5;
    public int enemyCount;
    
    public float spawnRange = 10f;
    public float maxDistance = 2.5f;
    public float restrictedSpawnRange;
    public float spawnRangeMaxLimit;

    private float playerToEnemyRange;

    void Start()
    {
        SpawnEnemyWave(spawnEnemyCount);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Target>().Length;
        if (enemyCount == 0)
        {
            spawnEnemyCount += Random.Range(1, 11);
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
            playerToEnemyRange = Vector3.Distance(player.transform.position, hit.position);

            if (playerToEnemyRange > restrictedSpawnRange && center.GetComponent<Collider>().bounds.Contains(hit.position))
            {
                if (!pressureTank.GetComponent<Collider>().bounds.Contains(hit.position))
                {
                    return hit.position;
                }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.transform.position, spawnRangeMaxLimit);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.transform.position, restrictedSpawnRange);
    }
}
