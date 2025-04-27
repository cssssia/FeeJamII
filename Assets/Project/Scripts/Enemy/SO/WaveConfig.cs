using UnityEngine;

[CreateAssetMenu(fileName = "CFG_Wave_0", menuName = "Scriptable Objects/Wave")]
public class WaveConfig : ScriptableObject
{
    public AnimationCurve enemySpeedCurve;
    public AnimationCurve enemySpawnDelayCurve;

    public int AmountOfEnemies
    {
        get { return m_amountOfEnemies * 3; }
    }

    [SerializeField] private int m_amountOfEnemies = 15;
}