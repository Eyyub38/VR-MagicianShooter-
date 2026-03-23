using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<Enemy> enemies;
    [SerializeField] int numEnemiesToSpawn = 5;
    [SerializeField] float spawnInterval = 2f;

    [Header( "Size of Spawner" )]
    [SerializeField] float sizeX = 25;
    [SerializeField] float sizeZ = 15;

    int enemiesSpawned = 0;
    float timeSinceLastSpawn = 0f;
    bool isEnemiesAlive = true;

    //TODO: Enemy Types and new spawn algorithm variables will be added

    public int EnemiesSpawned { get; set; }

    void Update() {
        if(enemyPrefab == null)
            return;
        if(GameManager.i == null) {
            Debug.LogWarning( "EnemySpawner: GameManager.i is null - skipping update" );
            return;
        }
        if(GameManager.i.CurrentGameState != GameStates.Playing)
            return;

        timeSinceLastSpawn += Time.deltaTime;

        isEnemiesAlive = GameObject.FindGameObjectsWithTag( "Enemy" ).Length > 0;

        if(!isEnemiesAlive && enemiesSpawned >= numEnemiesToSpawn) {
            enemiesSpawned = 0;
        }

        if(enemiesSpawned < numEnemiesToSpawn && timeSinceLastSpawn >= spawnInterval) {
            SpawnOneEnemy();
            timeSinceLastSpawn -= spawnInterval;
        }
    }

    void SpawnOneEnemy() {
        var spawnerPos = transform.position;
        float spawnX = Random.Range( spawnerPos.x - sizeX, spawnerPos.x + sizeX );
        float spawnZ = Random.Range( spawnerPos.z - sizeZ, spawnerPos.z + sizeZ );

        Enemy enemy = GetRandomEnemy();

        GameObject newEnemy = Instantiate( enemyPrefab, new Vector3( spawnX, 1f, spawnZ ), Quaternion.identity );
        newEnemy.GetComponent<EnemyController>().Enemy = enemy;

        Vector3 spawnPos = new Vector3( spawnX, 1f, spawnZ );
        Instantiate( enemyPrefab, spawnPos, Quaternion.identity );
        enemiesSpawned++;
    }

    public void IsAnyEnemyAlive() {
        isEnemiesAlive = GameObject.FindGameObjectsWithTag( "Enemy" ).Length > 0;
        if(!isEnemiesAlive) {
            enemiesSpawned = 0;
        }
    }

    Enemy GetRandomEnemy() {
        int totalWeight = 0;
        foreach(var enemy in enemies) {
            totalWeight += enemy.SpawnWeight;
        }

        int random = Random.Range( 0, totalWeight );
        int weight = 0;

        foreach(var enemy in enemies) {
            weight += enemy.SpawnWeight;
            if(random <= weight) {
                return enemy;
            }
        }
        return enemies[0];
    }

    public void CleanEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach(GameObject enemy in enemies) {
            Destroy( enemy );
        }
    }
}
