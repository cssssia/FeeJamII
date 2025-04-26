using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyLane
{
    Default,
    Top,
    Middle,
    Bottom
}

public class EnemyManager : Singleton<EnemyManager>
{
    // public bool spawnEnemies = true;
    // [SerializeField] private Vector2 m_enemySpawnRangeTimer;

    // [SerializeField] private GameObject m_enemyPrefab;
    // [SerializeField] private List<Sprite> m_enemySprites;

    // [SerializeField] private List<Transform> m_enemySpawnTransforms;

    // void Start()
    // {
    //     if (spawnEnemies)
    //         StartCoroutine(SpawnEnemy(0f));
    // }

    // private EnemyLane RandomizeLane()
    // {
    //     return (EnemyLane)Random.Range(1, 4);
    // }

    // private EnemyController InstantiateEnemy(Vector2 p_startPosition)
    // {
    //     m_enemyPrefab.GetComponent<EnemyController>().enemySprite.sprite = m_enemySprites[Random.Range(0, 3)];

    //     return Instantiate(m_enemyPrefab, p_startPosition, Quaternion.identity).GetComponent<EnemyController>();
    // }

    // private IEnumerator SpawnEnemy(float p_timeToWaitForSpawn)
    // {
    //     EnemyLane l_thisEnemyLane = RandomizeLane();

    //     yield return new WaitForSeconds(p_timeToWaitForSpawn);

    //     SetEnemySpawnPos(l_thisEnemyLane);
    // }

    // private void SetEnemySpawnPos(EnemyLane p_thisEnemyLane)
    // {
    //     switch (p_thisEnemyLane)
    //     {
    //         case EnemyLane.Top:
    //             InstantiateEnemy(m_enemySpawnTransforms[0].position);
    //             break;
    //         case EnemyLane.Middle:
    //             InstantiateEnemy(m_enemySpawnTransforms[1].position);
    //             break;
    //         case EnemyLane.Bottom:
    //             InstantiateEnemy(m_enemySpawnTransforms[2].position);
    //             break;
    //     }

    //     if (spawnEnemies) StartCoroutine(SpawnEnemy(Random.Range(m_enemySpawnRangeTimer.x, m_enemySpawnRangeTimer.y)));
    // }

    public Action<int> OnEnemyReachLaneEnd;

    public void EnemyReachLaneEnd()
    {
        OnEnemyReachLaneEnd?.Invoke(1);
    }

}
