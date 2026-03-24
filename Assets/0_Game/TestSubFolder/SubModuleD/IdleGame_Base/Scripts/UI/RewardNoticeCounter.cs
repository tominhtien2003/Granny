using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNoticeCounter : MonoBehaviour
{
    public GameObject m_redNotice;
    public void SetRedNotice(bool state)
    {
        if (m_redNotice.activeSelf != state) m_redNotice.SetActive(state);
    }
    public void SetCounter(float time)
    {
        StopAllCoroutines();
        StartCoroutine(CounterCoroutine(time));
    }
    public void StopCounter()
    {
        StopAllCoroutines();
    }
    private IEnumerator CounterCoroutine(float time)
    {
        SetRedNotice(false);
        yield return new WaitForSeconds(time);
        SetRedNotice(true);
    }
}
