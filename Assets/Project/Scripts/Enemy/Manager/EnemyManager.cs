using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

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

    public float startingMinSpawnDelay;

    [FormerlySerializedAs("startingMaxSpeed")]
    public float startingSpeed;

    [FormerlySerializedAs("startingSpawnDelay")]
    public float startingMaxSpawnDelay;

    [SerializeField, ReadOnly] private float m_currentMinEnemySpawnDelay;

    [SerializeField, ReadOnly] private float m_currentMaxEnemySpawnDelay;

    [FormerlySerializedAs("m_currentMaxEnemySpeed")] [SerializeField, ReadOnly]
    private float m_currentEnemySpeed;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_currentMinEnemySpawnDelay = startingMinSpawnDelay;
        m_currentMaxEnemySpawnDelay = startingMaxSpawnDelay;
        m_currentEnemySpeed = startingSpeed;
        m_currentWaveConfig = m_progressionConfig.GetCurrentWaveByWaveNumber(0);
    }

    public int AmountOfEnemiesThisWave
    {
        get { return m_currentWaveConfig.AmountOfEnemies; }
    }

    public float CurrentEnemySpeed
    {
        get { return m_currentEnemySpeed; }
    }

    public float CurrentMinEnemySpawnDelay
    {
        get { return m_currentMinEnemySpawnDelay; }
    }

    public float CurrentMaxEnemySpawnDelay
    {
        get { return m_currentMaxEnemySpawnDelay; }
    }

    private int enemiesSpawnedThisWave;

    public void EnemySpawned(EnemyController p_enemyController)
    {
        enemiesSpawnedThisWave++;
        // decrease spawn delay
        m_currentMinEnemySpawnDelay = Mathf.Max(.05f,
            m_currentMinEnemySpawnDelay - m_currentWaveConfig.SpawnDelay(enemiesSpawnedThisWave));
        m_currentMaxEnemySpawnDelay = Mathf.Max(.1f,
            m_currentMaxEnemySpawnDelay - m_currentWaveConfig.SpawnDelay(enemiesSpawnedThisWave));

        // increase speed
        m_currentEnemySpeed += m_currentWaveConfig.EnemySpeed(enemiesSpawnedThisWave);
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