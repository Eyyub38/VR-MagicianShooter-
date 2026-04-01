using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    [SerializeField] List<WaveData> waves;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] SpellMachine spellMachine;

    float timer = 0f;
    WaveData currentWave;

    List<WaveData> runtimeWaves = new List<WaveData>();

    public static WaveManager i { get; private set; }
    public WaveData CurrentWave => currentWave;

    void Awake() {
        runtimeWaves.Clear();
        foreach(var wave in waves) {
            if(wave == null) continue;
            runtimeWaves.Add( Instantiate( wave ) );
        }
        i = this;
    }

    void Update() {
        if(runtimeWaves.Count > 0) {
            HandleWave( runtimeWaves[0] );
        }
    }

    void HandleWave(WaveData wave) {
        spellMachine.CheckAndRefillBag( wave );
        currentWave = wave;

        if(wave.EnemyData.Count <= 0) {
            runtimeWaves.Remove( wave );
            Destroy( wave );
            currentWave = null;
            return;
        }

        timer += Time.deltaTime;

        if(timer >= wave.SpawnPeriod) {
            int randomIndex = Random.Range( 0, wave.EnemyData.Count );
            var enemyData = wave.EnemyData[randomIndex];

            enemySpawner.SpawnOneEnemy( enemyData.Element );
            enemyData.Quantity--;

            if(enemyData.Quantity <= 0) {
                wave.EnemyData.RemoveAt( randomIndex );
            }

            timer = 0f;
            spellMachine.CleanBag();
        }
    }
}