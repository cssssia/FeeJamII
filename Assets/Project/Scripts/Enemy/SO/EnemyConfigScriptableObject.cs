using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigScriptableObject", menuName = "Scriptable Objects/EnemyConfigScriptableObject")]
public class EnemyConfigScriptableObject : ScriptableObject
{
    public bool spawnEnemies;
    public List<bool> spawnLaneEnemies;

    public float waveDuration;

    public float waitTimeBeforeStartSpawn;
    public bool destroyEnemiesWhenDamage;
    public Vector2 enemySpawnRangeTimer;
    public float enemyVelocity;
}
