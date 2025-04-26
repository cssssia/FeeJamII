using System;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    [SerializeField] private int m_life = 10;

    void Start()
    {
        EnemyManager.Instance.OnEnemyReachLaneEnd += OnEnemyReachLaneEnd;
    }

    private void OnEnemyReachLaneEnd(int p_point)
    {
        m_life -= p_point;
        Debug.Log(m_life);
    }
}
