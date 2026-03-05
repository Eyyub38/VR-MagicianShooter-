using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int numEnemiesToSpawn = 5;
    [SerializeField] float spawnInterval = 2f;

    int enemiesSpawned = 0;
    float timeSinceLastSpawn = 0f;
    bool isEnemiesAlive = true;

    public int EnemiesSpawned { get; set; }

    void Update()
    {
        if (enemyPrefab == null)
            return;
        if (GameManager.i == null)
        {
            Debug.LogWarning("EnemySpawner: GameManager.i is null - skipping update");
            return;
        }
        if (GameManager.i.CurrentGameState != GameStates.Playing)
            return;

        timeSinceLastSpawn += Time.deltaTime;

        isEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

        if (!isEnemiesAlive && enemiesSpawned >= numEnemiesToSpawn)
        {
            enemiesSpawned = 0;
        }

        if (enemiesSpawned < numEnemiesToSpawn && timeSinceLastSpawn >= spawnInterval)
        {
            SpawnOneEnemy();
            timeSinceLastSpawn -= spawnInterval;
        }
    }

    void SpawnOneEnemy()
    {
        var spawnerPos = transform.position;
        float spawnX = Random.Range(spawnerPos.x - 25f, spawnerPos.x + 25f);
        float spawnZ = Random.Range(spawnerPos.z - 20f, spawnerPos.z + 20f);

        Vector3 spawnPos = new Vector3(spawnX, 1f, spawnZ);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesSpawned++;
    }

    public void IsAnyEnemyAlive()
    {
        isEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;
        if (!isEnemiesAlive)
        {
            enemiesSpawned = 0;
        }
    }

    public void CleanEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
