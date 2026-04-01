using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<GameObject> enemyPrefabs;

    [Header( "Size of Spawner" )]
    [SerializeField] float sizeX = 25;
    [SerializeField] float sizeZ = 15;

    int enemiesSpawned = 0;
    bool isEnemiesAlive = false;

    public bool IsEnemiesAlive => isEnemiesAlive;
    public int EnemiesSpawned {
        get => enemiesSpawned;
        set => enemiesSpawned = value;
    }

    public void SpawnOneEnemy(ElementID element) {
        var spawnerPos = transform.position;
        float spawnX = Random.Range( spawnerPos.x - sizeX, spawnerPos.x + sizeX );
        float spawnZ = Random.Range( spawnerPos.z - sizeZ, spawnerPos.z + sizeZ );

        GameObject enemyPrefab = GetPrefabFromType( element );
        if(enemyPrefab == null) {
            Debug.LogError( "EnemySpawner: No prefab found for element: " + element );
            return;
        }

        GameObject newEnemy = Instantiate( enemyPrefab, new Vector3( spawnX, 0f, spawnZ ), Quaternion.identity );
        enemiesSpawned++;
    }

    public void IsAnyEnemyAlive() {
        isEnemiesAlive = GameObject.FindGameObjectsWithTag( "Enemy" ).Length > 0;
        if(!isEnemiesAlive) {
            enemiesSpawned = 0;
        }
    }

    public void CleanEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach(GameObject enemy in enemies) {
            Destroy( enemy );
        }
    }

    public GameObject GetPrefabFromType(ElementID element) {
        return enemyPrefabs.Find( prefab => {
            EnemyController enemyComponent = prefab.GetComponent<EnemyController>();
            return enemyComponent != null && enemyComponent.Enemy.Element == element;
        } );
    }
}
