using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CFG_Progression", menuName = "Scriptable Objects/Difficulty Progression")]
public class DifficultyProgressionConfig : ScriptableObject
{
    public List<WaveReference> waves;

    public WaveConfig GetCurrentWaveByWaveNumber(int p_waveNumber)
    {
        for (int i = 0; i < waves.Count; i++)
        {
            WaveReference l_waveRef = waves[i];
            if (p_waveNumber > l_waveRef.levelRange.x && p_waveNumber < l_waveRef.levelRange.y)
                return l_waveRef.wave;
        }

        return waves[^1].wave;
    }
}

[Serializable]
public class WaveReference
{
    [Expandable] public WaveConfig wave;

    [MinMaxSlider(0, 50), AllowNesting] public Vector2Int levelRange;
}