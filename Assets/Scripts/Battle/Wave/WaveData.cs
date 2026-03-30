using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu( fileName = "New Wave", menuName = "Wave" )]
public class WaveData : ScriptableObject {
    [SerializeField] List<WaveEnemyData> enemyData;
    [SerializeField] float spawnPeriod;

    public float SpawnPeriod => spawnPeriod;
    public List<WaveEnemyData> EnemyData => enemyData;
}

[System.Serializable]
public class WaveEnemyData {
    [SerializeField] ElementID element;
    [SerializeField] int quantity;

    public ElementID Element => element;
    public int Quantity { get => quantity; set => quantity = value; }
}
