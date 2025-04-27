using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CronometerController : Singleton<CronometerController>
{
    [SerializeField] private List<Sprite> m_numbersSprites;
    [SerializeField] private Image m_tenSpriteRenderer;
    [SerializeField] private Image m_unitSpriteRenderer;
    [SerializeField] private Animator animator;

    public int laneTime = 0;
    private int aux = 0;

    public void StartCronometer(int p_seconds)
    {
        if (laneTime == 0 && aux == 0) laneTime = p_seconds;
        else if (laneTime > p_seconds) laneTime = p_seconds;

        aux++;

        if (aux >= 3)
        {
            StartCoroutine(RunCronometer(laneTime));
            aux = 0;
            laneTime = 0;
        }
    }

    public IEnumerator RunCronometer(int p_seconds)
    {
        for (int i = p_seconds; i >= 0; i--)
        {
            if (i >= 10)
            {
                int l_ten = int.Parse(i.ToString().Substring(0, 1));
                int l_unit = int.Parse(i.ToString().Substring(1, 1));

                m_tenSpriteRenderer.sprite = m_numbersSprites[l_ten];
                m_unitSpriteRenderer.sprite = m_numbersSprites[l_unit];
            }
            else
            {
                m_tenSpriteRenderer.sprite = m_numbersSprites[0];
                m_unitSpriteRenderer.sprite = m_numbersSprites[i];
            }

            animator.enabled = true;

            yield return new WaitForSeconds(1f);
        }

        animator.enabled = false;

    }
}