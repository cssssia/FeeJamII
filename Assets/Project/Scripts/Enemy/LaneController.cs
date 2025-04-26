using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    public bool spawnLaneEnemies = true;

    [SerializeField] private Vector2 m_enemySpawnRangeTimer;
    [SerializeField] private int m_laneNumber;

    [SerializeField] private EnemyScriptableObject enemiesSO;

    void Start()
    {
        EnemyManager.Instance.OnCanSpawnEnemies += OnCanSpawnEnemies;

        if (spawnLaneEnemies)
            StartCoroutine(SpawnEnemy(Random.Range(0f, m_enemySpawnRangeTimer.x)));
    }

    private void OnCanSpawnEnemies(bool p_spawnEnemies)
    {
        spawnLaneEnemies = p_spawnEnemies;

        if (p_spawnEnemies)
            StartCoroutine(SpawnEnemy(Random.Range(0f, m_enemySpawnRangeTimer.x)));
    }

    private IEnumerator SpawnEnemy(float p_timeToWaitForSpawn)
    {
        yield return new WaitForSeconds(p_timeToWaitForSpawn);

        if (spawnLaneEnemies)
        {
            EnemyManager.Instance.EnemySpawned(InstantiateEnemy(transform.position));

            StartCoroutine(SpawnEnemy(Random.Range(m_enemySpawnRangeTimer.x, m_enemySpawnRangeTimer.y)));
        }

    }

    private EnemyController InstantiateEnemy(Vector2 p_startPosition)
    {
        int enemyType = Random.Range(0, 3);

        return Instantiate(enemiesSO.prefab, p_startPosition, Quaternion.identity).GetComponent<EnemyController>().Setup(m_laneNumber, enemiesSO.sprites[enemyType], enemiesSO.animators[enemyType], "Enemy" + enemyType);
    }

}
