using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject pressureTank;
    public GameObject player;
    public GameObject center;

    public TextMeshProUGUI roundEndText;

    public int spawnEnemyCount;
    public int waveCount;
    public int enemyCount;
    
    public float spawnRange = 10f;
    public float maxDistance = 2.5f;
    public float restrictedSpawnRange;
    public float spawnRangeMaxLimit;

    private float playerToEnemyRange;
    private float centerToHitPosition;

    private float roundEndTime = 5f;

    private bool gameEnded;

    void Start()
    {
        //spawnEnemyCount = Random.Range(1, 11);
        waveCount = Random.Range(2, 6);
        SpawnEnemyWave(spawnEnemyCount);
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Target>().Length;

        if (waveCount == 0)
        {
            gameEnded = true;
            StartCoroutine(RoundEnd());
        }

        if (enemyCount == 0 && !gameEnded)
        {
            waveCount--;

            if (waveCount != 0)
            {
                spawnEnemyCount += Random.Range(1, 11);
                SpawnEnemyWave(spawnEnemyCount);
            }
        }
    }

    private IEnumerator RoundEnd()
    {
        roundEndText.gameObject.SetActive(true);

        yield return new WaitForSeconds(roundEndTime);

        // Reset Health 
        Player.health = 100;

        roundEndText.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
