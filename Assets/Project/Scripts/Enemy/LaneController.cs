using System.Collections;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    public bool spawnLaneEnemies = true;

    [SerializeField] private int m_laneNumber;

    [SerializeField] private EnemyScriptableObject enemiesSO;

    void Start()
    {
        EnemyManager.Instance.OnCanSpawnEnemies += OnCanSpawnEnemies;
        spawnLaneEnemies = EnemyManager.Instance.spawnEnemies;

        if (spawnLaneEnemies)
            StartCoroutine(SpawnEnemy(EnemyManager.Instance.CurrentEnemySpawnDelay));
    }

    private void OnCanSpawnEnemies(bool p_spawnEnemies)
    {
        spawnLaneEnemies = p_spawnEnemies;

        if (p_spawnEnemies)
            StartCoroutine(SpawnEnemy(EnemyManager.Instance.CurrentEnemySpawnDelay));
    }

    private IEnumerator SpawnEnemy(float p_timeToWaitForSpawn)
    {
        yield return new WaitForSeconds(p_timeToWaitForSpawn);

        if (spawnLaneEnemies)
        {
            EnemyManager.Instance.EnemySpawned(InstantiateEnemy(transform.position));

            StartCoroutine(SpawnEnemy(EnemyManager.Instance.CurrentEnemySpawnDelay));
        }
    }

    private EnemyController InstantiateEnemy(Vector2 p_startPosition)
    {
        int enemyType = Random.Range(0, 3);

        return Instantiate(enemiesSO.prefab, p_startPosition, Quaternion.identity)
            .GetComponent<EnemyController>()
            .Setup(m_laneNumber, enemiesSO.sprites[enemyType],
                enemiesSO.animators[enemyType],
                "Enemy" + enemyType, (EnemyType)enemyType + 1);
    }
}