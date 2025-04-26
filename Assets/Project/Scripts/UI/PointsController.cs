using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : Singleton<PointsController>
{
    [SerializeField] private int m_life = 10;
    [SerializeField] private int m_numOfHearts;
    [SerializeField] private int m_penality;

    [SerializeField] private TextMeshProUGUI m_lifeText;
    [SerializeField] private TextMeshProUGUI m_penalityText;
    [SerializeField] private List<Image> m_heartContainers;

    void Start()
    {
        EnemyManager.Instance.OnEnemyReachLaneEnd += OnEnemyReachLaneEnd;

        for (int i = 0; i < m_heartContainers.Count; i++)
        {
            if (i < m_numOfHearts)
            {
                m_heartContainers[i].enabled = true;
            }
            else
            {
                m_heartContainers[i].enabled = false;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < m_heartContainers.Count; i++)
        {
            if (i < m_life)
            {
                m_heartContainers[i].enabled = true;
            }
            else
            {
                m_heartContainers[i].enabled = false;
            }
        }
    }

    private void OnEnemyReachLaneEnd(int p_point)
    {
        m_life -= p_point;
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
