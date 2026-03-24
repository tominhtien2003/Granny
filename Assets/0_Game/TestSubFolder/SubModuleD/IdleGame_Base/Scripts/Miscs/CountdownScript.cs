using Cysharp.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownScript : MonoBehaviour
{
    public TMP_Text m_display;
    private int m_cnt;
    public Action OnCountdownComplete;
    public void StartCount(int count)
    {
        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine(count));
    }
    
    public void AddTime(int t)
    {
        if (!gameObject.activeSelf) return;
        m_cnt += t;
        if (m_cnt < 0) m_cnt = 0;
        SetCountdownText(m_cnt);
    }
    public void ContinueCountdown()
    {
        StartCount(m_cnt);
    }
    private IEnumerator CountdownCoroutine(int countdown)
    {
        m_cnt = countdown;
        while (m_cnt > 0)
        {
            SetCountdownText(m_cnt);
            yield return Yielder.Get(1);
            m_cnt--;
        }
        OnCountdownComplete?.Invoke();
    }
    public void StopCounting()
    {
        StopAllCoroutines();
    }
    public int CurrentCountdown()
    {
        return m_cnt;
    }
    public void SetCountdownText(int cnt)
    {
        int m = cnt / 60;
        int s = cnt % 60;
        m_display.SetTextFormat("{0:D2}:{1:D2}", m, s);
    }
}
