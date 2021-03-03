using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject pressureTank;
    public GameObject player;
    public GameObject center;

    public int spawnEnemyCount = 5;
    public int waveCount;
    public int enemyCount;
    
    public float spawnRange = 10f;
    public float maxDistance = 2.5f;
    public float restrictedSpawnRange;
    public float spawnRangeMaxLimit;

    private float playerToEnemyRange;
    private float centerToHitPosition;

    void Start()
    {
        waveCount = Random.Range(1, 2);
        SpawnEnemyWave(spawnEnemyCount);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Target>().Length;

        if (waveCount == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 > EditorBuildSettings.scenes.Length)
                Debug.Log("Finish");
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (enemyCount == 0 && waveCount > 0)
        {
            waveCount--;
            spawnEnemyCount += Random.Range(1, 11);
            SpawnEnemyWave(spawnEnemyCount);
        }
    }

    private Vector3 GenerateRandomSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0f, spawnPosZ);

        if (NavMesh.SamplePosition(randomPos + center.transform.position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            playerToEnemyRange = Vector3.Distance(player.transform.position, hit.position);
            centerToHitPosition = Vector3.Distance(center.transform.position, hit.position);

            if (playerToEnemyRange > restrictedSpawnRange && centerToHitPosition < spawnRangeMaxLimit)
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.transform.position, restrictedSpawnRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.transform.position, spawnRangeMaxLimit);
    }
}
