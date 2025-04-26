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
    public bool spawnEnemies = false;
    public bool destroyEnemiesWhenDamage = true;
    public float waitTimeBeforeStartSpawn = 3f;
    private List<EnemyController> m_enemiesInGame = new();

    public Action<int> OnEnemyReachLaneEnd;
    public Action<bool> OnCanSpawnEnemies;

    public void EnemySpawned(EnemyController p_enemyController)
    {
        m_enemiesInGame.Add(p_enemyController);
    }

    public void EnemyDied(EnemyController p_enemyController)
    {
        m_enemiesInGame.Remove(p_enemyController);
    }

    public void EnemyReachLaneEnd()
    {
        OnEnemyReachLaneEnd?.Invoke(1);

        if (destroyEnemiesWhenDamage)
        {
            SpawnEnemies(false);
            for (int i = m_enemiesInGame.Count - 1; i >= 0; i--)
            {
                Destroy(m_enemiesInGame[i].gameObject);
                m_enemiesInGame.RemoveAt(i);
            }
        }

        StartCoroutine(StartSpawn());
    }

    public void PenalityOccured()
    {
        if (destroyEnemiesWhenDamage)
        {
            SpawnEnemies(false);
            for (int i = m_enemiesInGame.Count - 1; i >= 0; i--)
            {
                Destroy(m_enemiesInGame[i].gameObject);
                m_enemiesInGame.RemoveAt(i);
            }
        }

        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(waitTimeBeforeStartSpawn);

        SpawnEnemies(true);
    }

    public void SpawnEnemies(bool p_spawnEnemies)
    {
        spawnEnemies = p_spawnEnemies;
        OnCanSpawnEnemies?.Invoke(p_spawnEnemies);
    }

    public void SpawnEnemies(bool p_spawnEnemies, bool p_isWave)
    {
        if (p_isWave) StopAllCoroutines();
        spawnEnemies = p_spawnEnemies;
        OnCanSpawnEnemies?.Invoke(p_spawnEnemies);
    }
}
