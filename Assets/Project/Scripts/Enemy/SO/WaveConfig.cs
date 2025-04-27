using UnityEngine;

[CreateAssetMenu(fileName = "CFG_Wave_0", menuName = "Scriptable Objects/Wave")]
public class WaveConfig : ScriptableObject
{
    public float speedIncrease;
    public AnimationCurve speedIncreaseCurve;
    public float spawnDelayDecrease;
    public AnimationCurve spawnDelayCurve;

    public float SpawnDelay(int p_amountSpawnedThisWave)
    {
        return
            spawnDelayDecrease; // spawnDelayCurve.Evaluate((float)p_amountSpawnedThisWave / AmountOfEnemies) * spawnDelayDecrease;
    }

    public float EnemySpeed(int p_amountSpawnedThisWave)
    {
        return
            speedIncrease; // speedIncreaseCurve.Evaluate((float)p_amountSpawnedThisWave / AmountOfEnemies) * speedIncrease;
    }

    public int AmountOfEnemies
    {
        get { return m_amountOfEnemies * 3; }
    }

    [SerializeField] private int m_amountOfEnemies = 100;
}