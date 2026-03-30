using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<GameObject> enemyPrefabs;
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

    public void SpawnOneEnemy(ElementID element) {
        var spawnerPos = transform.position;
        float spawnX = Random.Range( spawnerPos.x - sizeX, spawnerPos.x + sizeX );
        float spawnZ = Random.Range( spawnerPos.z - sizeZ, spawnerPos.z + sizeZ );

        GameObject enemyPrefab = GetPrefabFromType( new Enemy { Element = element } );
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

    public GameObject GetPrefabFromType(Enemy enemy) {
        switch(enemy.Element) {
            case ElementID.Fire:
                return enemyPrefabs.Find( prefab => prefab.GetComponent<Enemy>().Element == ElementID.Fire );
            case ElementID.Ice:
                return enemyPrefabs.Find( prefab => prefab.GetComponent<Enemy>().Element == ElementID.Ice );
            case ElementID.Wind:
                return enemyPrefabs.Find( prefab => prefab.GetComponent<Enemy>().Element == ElementID.Wind );
            case ElementID.Lightning:
                return enemyPrefabs.Find( prefab => prefab.GetComponent<Enemy>().Element == ElementID.Lightning );
            case ElementID.Earth:
                return enemyPrefabs.Find( prefab => prefab.GetComponent<Enemy>().Element == ElementID.Earth );
            default:
                return null;
        }
    }
}
