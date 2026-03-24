using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyReceiveAnimator : MonoBehaviour
{
    public Transform m_moneyTarget;
    public MoneyController m_controller;

    public List<Transform> m_moneyGraphics;

    private List<MotionHandle> m_motionHandleList = new List<MotionHandle>();
    private WaitForSeconds m_wfs;
    public int m_increasePhases = 30;
    public float m_increaseTime = 3f;

    private Coroutine m_moneyGraphicCoroutine;
    private Coroutine m_moneyIncreaseCoroutine;

    public double m_beforeAmount, m_afterAmount;
    public bool m_playSound;
    private void Awake()
    {
        for (int i = 0; i < m_moneyGraphics.Count; i++)
        {
            m_motionHandleList.Add(new MotionHandle());
        }
        float t = m_increaseTime / m_increasePhases;
        //this.LogError(t);
        m_wfs = new WaitForSeconds(t);

    }
    public void StartAnimate(double money)
    {
        m_beforeAmount = m_controller.m_totalMoney;
        m_controller.AddToTotalMoney(money, false);
        m_afterAmount = m_controller.m_totalMoney;
        AnimateMoneyReceive();
    }
    public void AnimateMoneyReceive()
    {
        if (m_moneyGraphicCoroutine != null) StopCoroutine(m_moneyGraphicCoroutine);
        if (m_moneyIncreaseCoroutine != null) StopCoroutine(m_moneyIncreaseCoroutine);

        m_moneyGraphicCoroutine = StartCoroutine(MoneyReceiveCoroutine());
    }

    public IEnumerator MoneyReceiveCoroutine()
    {
        m_controller.UpdateView(m_beforeAmount);
        for (int i = 0; i < m_moneyGraphics.Count; i++)
        {
            Transform money = m_moneyGraphics[i];
            money.gameObject.SetActive(true);
            float delay = i * 0.1f;

            m_motionHandleList[i].TryCancel();
            m_motionHandleList[i] = LMotion.Create(Vector3.zero, (Vector3)Random.insideUnitCircle * 400f, .3f).WithEase(Ease.InQuad).BindToLocalPosition(money);
                /*LSequence.Create()
                .AppendInterval(delay)
                .Append(LMotion.Create(Vector3.zero, (Vector3)Random.insideUnitCircle * 400f, .3f).WithEase(Ease.InQuad).BindToLocalPosition(money))
                .Join(LMotion.Create(Vector3.one * .1f, Vector3.one, .3f).WithEase(Ease.InQuart).BindToLocalScale(money))
                .Run();*/
        }

        yield return m_motionHandleList[m_moneyGraphics.Count - 1].ToYieldInstruction();
        m_moneyIncreaseCoroutine = StartCoroutine(IncreaseMoneyCoroutine());
        Vector3 targetDes = m_moneyGraphics[0].parent.InverseTransformPoint(m_moneyTarget.position);

        for (int i = 0; i < m_moneyGraphics.Count; i++)
        {
            Transform money = m_moneyGraphics[i];
            float delay = i * .05f;
            m_motionHandleList[i].TryCancel();

            m_motionHandleList[i] = LMotion.Create(money.localPosition, targetDes, 1f).WithDelay(delay).WithEase(Ease.InQuart).WithOnComplete(()=>OnCoinReached(money)).BindToLocalPosition(money).AddTo(money);
        }
        m_moneyGraphicCoroutine = null;
    }
    private void OnCoinReached(Transform money)
    {
        if (m_playSound) AudioManager.Instance.PlayCollectSound();
        money.gameObject.SetActive(false);
        LMotion.Create(Vector3.one, Vector3.one * 1.2f, .1f).WithLoops(2, LoopType.Yoyo).BindToLocalScale(m_moneyTarget);
    }
    IEnumerator IncreaseMoneyCoroutine()
    {
        double increaseAmount = m_afterAmount - m_beforeAmount;

        double increasePerPhase = increaseAmount / m_increasePhases;

        if (increasePerPhase < 1) increasePerPhase = 1;

        yield return Yielder.Get(1f);
        while (m_beforeAmount < m_afterAmount)
        {
            m_beforeAmount += increasePerPhase;
            if (m_beforeAmount > m_afterAmount) m_beforeAmount = m_afterAmount;
            m_controller.UpdateView(m_beforeAmount);

            yield return m_wfs;
        }
        m_controller.UpdateView(m_controller.m_totalMoney);
        m_moneyIncreaseCoroutine = null;
    }
}
