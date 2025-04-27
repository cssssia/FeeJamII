using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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
    [SerializeField] private List<EnemyController> m_enemiesInGame = new();

    public Action<int> OnEnemyReachLaneEnd;
    public Action<bool> OnCanSpawnEnemies;
    [SerializeField] private DifficultyProgressionConfig m_progressionConfig;
    [SerializeField] private WaveConfig m_currentWaveConfig;
    [SerializeField, ReadOnly] private int currentWave;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_currentWaveConfig = m_progressionConfig.GetCurrentWaveByWaveNumber(0);
    }

    public int AmountOfEnemiesThisWave
    {
        get { return m_currentWaveConfig.AmountOfEnemies; }
    }

    public float CurrentEnemySpeed
    {
        get
        {
            return 1 + m_currentWaveConfig.enemySpeedCurve.Evaluate((float)enemiesSpawnedThisWave /
                                                                    AmountOfEnemiesThisWave) * currentWave;
        }
    }

    public float CurrentEnemySpawnDelay
    {
        get
        {
            return m_currentWaveConfig.enemySpawnDelayCurve.Evaluate(1 - (float)enemiesSpawnedThisWave /
                AmountOfEnemiesThisWave);
        }
    }

    private int enemiesSpawnedThisWave;

    public void EnemySpawned(EnemyController p_enemyController)
    {
        enemiesSpawnedThisWave++;
        if (enemiesSpawnedThisWave >= AmountOfEnemiesThisWave) NextWave();
        m_enemiesInGame.Add(p_enemyController);
    }

    private void NextWave()
    {
        currentWave++;
        m_currentWaveConfig = m_progressionConfig.GetCurrentWaveByWaveNumber(currentWave);
    }

    public void EnemyDied(EnemyController p_enemyController)
    {
        m_enemiesInGame.Remove(p_enemyController);
        p_enemyController.animator.Play("Die");
    }

    public void EnemyReachLaneEnd()
    {
        OnEnemyReachLaneEnd?.Invoke(1);

        if (destroyEnemiesWhenDamage)
        {
            SpawnEnemies(false);
            for (int i = m_enemiesInGame.Count - 1; i >= 0; i--)
            {
                GameObject l_gameObject = m_enemiesInGame[i].gameObject;
                m_enemiesInGame[i].animator.Play("Die");

                m_enemiesInGame.RemoveAt(i);

                Destroy(l_gameObject);

                // Destroy(l_gameObject);
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
                GameObject l_gameObject = m_enemiesInGame[i].gameObject;

                m_enemiesInGame.RemoveAt(i);

                Destroy(l_gameObject);
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
}