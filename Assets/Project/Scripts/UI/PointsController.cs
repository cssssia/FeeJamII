using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PointsController : Singleton<PointsController>
{
    [SerializeField] private int m_life = 10;
    [SerializeField] private int m_penality;

    [SerializeField] private TextMeshProUGUI m_lifeText;
    [SerializeField] private TextMeshProUGUI m_penalityText;

    void Start()
    {
        EnemyManager.Instance.OnEnemyReachLaneEnd += OnEnemyReachLaneEnd;
    }

    private void OnEnemyReachLaneEnd(int p_point)
    {
        m_life -= p_point;
        m_lifeText.text = m_life.ToString();
    }

    public int Penality()
    {
        m_penality++;

        if (m_penality == 1) m_penalityText.text = "I";
        else if (m_penality == 2) m_penalityText.text = "II";
        else if (m_penality == 3) m_penalityText.text = "III";

        if (m_penality == 3)
        {
            m_life--;
            Debug.Log(m_life);
            m_penality = 0;
            EnemyManager.Instance.PenalityOccured();

            StartCoroutine(PenalityWarn());
        }

        return m_penality;
    }

    public IEnumerator PenalityWarn()
    {
        yield return new WaitForSeconds(0.25f);
        m_penalityText.text = "";
        yield return new WaitForSeconds(0.25f);
        m_penalityText.text = "III";
        yield return new WaitForSeconds(0.25f);
        m_penalityText.text = "";
        yield return new WaitForSeconds(0.25f);
        m_penalityText.text = "III";
        yield return new WaitForSeconds(0.25f);
        m_penalityText.text = "";
    }
}
