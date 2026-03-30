using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    [SerializeField] List<WaveData> waves;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] SpellMachine spellMachine;

    float timer = 0f;

    void Update() {
        if(waves.Count > 0) {
            HandleWave( waves[0] );
        }
    }

    void HandleWave(WaveData wave) {
        spellMachine.CheckAndRefillBag( wave );

        timer += Time.deltaTime;

        if(timer >= wave.SpawnPeriod) {
            for(int i = wave.EnemyData.Count - 1; i >= 0; i--) {
                var enemyData = wave.EnemyData[i];

                enemySpawner.SpawnOneEnemy( enemyData.Element );
                enemyData.Quantity--;

                if(enemyData.Quantity <= 0) {
                    wave.EnemyData.RemoveAt( i );
                }
            }
            timer = 0f;
            spellMachine.CleanBag();
        }

        if(wave.EnemyData.Count <= 0) {
            waves.Remove( wave );
        }
    }
}