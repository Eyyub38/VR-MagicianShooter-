using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu( fileName = "New Wave", menuName = "Wave" )]
public class WaveData : ScriptableObject {
    List<Enemy> enemies = new List<Enemy>();
    float spawnPeriod;

    public float SpawnPeriod => spawnPeriod;
}
