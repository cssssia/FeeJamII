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
        {
            float l_time = Random.Range(EnemyManager.Instance.CurrentMinEnemySpawnDelay,
            EnemyManager.Instance.CurrentMaxEnemySpawnDelay);

            CronometerController.Instance.StartCronometer((int)Mathf.Ceil(l_time));

            StartCoroutine(SpawnEnemy(l_time));
        }
    }

    private void OnCanSpawnEnemies(bool p_spawnEnemies)
    {
        spawnLaneEnemies = p_spawnEnemies;

        if(!p_spawnEnemies) StopAllCoroutines();

        if (p_spawnEnemies)
        {
            float l_time = Random.Range(EnemyManager.Instance.CurrentMinEnemySpawnDelay,
            EnemyManager.Instance.CurrentMaxEnemySpawnDelay);

            CronometerController.Instance.StartCronometer((int)Mathf.Ceil(l_time));

            StartCoroutine(SpawnEnemy(l_time));
        }
    }

    private IEnumerator SpawnEnemy(float p_time)
    {
        yield return new WaitForSeconds(p_time != 0 ? p_time : Random.Range(EnemyManager.Instance.CurrentMinEnemySpawnDelay,
            EnemyManager.Instance.CurrentMaxEnemySpawnDelay));

        if (spawnLaneEnemies)
        {
            EnemyManager.Instance.EnemySpawned(InstantiateEnemy(transform.position));

            StartCoroutine(SpawnEnemy(0));
        }
    }

    private EnemyController InstantiateEnemy(Vector2 p_startPosition)
    {
        int enemyType = Random.Range(0, 5);

        if (enemyType == 3 || enemyType == 4) enemyType = m_laneNumber;

        return Instantiate(enemiesSO.prefab, p_startPosition, Quaternion.identity)
            .GetComponent<EnemyController>()
            .Setup(m_laneNumber, enemiesSO.sprites[enemyType],
                enemiesSO.animators[enemyType],
                "Enemy" + enemyType, (EnemyType)enemyType + 1);
    }
}