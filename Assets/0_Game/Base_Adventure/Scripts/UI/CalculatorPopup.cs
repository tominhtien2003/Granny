using PrimeTween;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[Serializable]
public class CalculatorData
{
    public string m_targetValue;
    public UnityEvent m_onCorrect;
    public UnityEvent m_onWrong;
    public bool HideOnWrong;
}
public class CalculatorPopup : BasePopup
{
    int m_currentDigit = 0;
    public List<ButtonEffectLogic> m_numbers;
    public ButtonEffectLogic m_confirm;
    public ButtonEffectLogic m_clear;
    public List<TMP_Text> m_numberTexts;
    public List<char> m_numberChars;
    private static string[] m_numberStrings = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    public CalculatorData m_currentData;
    public Image m_image;
    public Sprite m_correctSprite;
    public Sprite m_wrongSprite;

    public int m_numOfDigits = 0;
#if UNITY_EDITOR
    [Button]
    void InitNumbers()
    {

        for (int i = 0; i <= 9; i++)
        {
            var v = m_numbers[i].GetComponentInChildren<TMP_Text>();
            v.SetText(i.ToString());
            EditorUtility.SetDirty(v);
        }
    }
#endif
    public override void InitialiseUI()
    {
        base.InitialiseUI();
        for (int i = 0; i < m_numbers.Count; i++)
        {
            int index = i;
            m_numbers[i].onClick.AddListener(() => AddDigit(index));

        }
        m_numberChars = new List<char>();
        for (int i = 0; i < 5; i++) m_numberChars.Add('#');
        m_numOfDigits = 0;
        m_confirm.onClick.AddListener(Confirm);
        m_clear.onClick.AddListener(Clear);
    }
    public void AddDigit(int digit)
    {
        if (m_currentDigit >= m_numOfDigits || m_isShowingResult) return;

        m_numberTexts[m_currentDigit].SetText(m_numberStrings[digit]);
        m_numberChars[m_currentDigit] = m_numberStrings[digit][0];
        m_currentDigit++;
    }
    public void InitPopup(CalculatorData data)
    {
        if (m_currentData == data) return;
        m_currentData = data;
        int num = m_currentData.m_targetValue.Length;
        while (m_numOfDigits < num)
        {
            m_numOfDigits++;
            m_numberTexts[m_numOfDigits - 1].enabled = true;

        }
        while (m_numOfDigits > num)
        {
            m_numberTexts[m_numOfDigits - 1].enabled = false;
            m_numOfDigits--;
        }
        Clear();
        Show();
    }
    private bool m_isCorrect;
    private bool m_isShowingResult = false;
    void ShowImage()
    {
        m_image.enabled = true;
        m_image.sprite = m_isCorrect ? m_correctSprite : m_wrongSprite;
        m_isShowingResult = true;

        Tween.Alpha(m_image, startValue: 0, 1, .7f, cycles: 2, cycleMode: CycleMode.Yoyo).OnComplete(DoneAnswering);
    }
    void DoneAnswering()
    {
        m_isShowingResult = false;
        m_image.enabled = false;
        if (m_isCorrect)
        {
            m_currentData.m_onCorrect.Invoke();
            Hide();
        }
        else
        {
            m_currentData.m_onWrong.Invoke();
            if (m_currentData.HideOnWrong) Hide();
        }

        Clear();
    }
    void Clear()
    {
        if (m_isShowingResult) return;
        foreach (var v in m_numberTexts) v.SetText("#");
        for (int i = 0; i < m_numberChars.Count; i++) m_numberChars[i] = '#';
        m_currentDigit = 0;
    }
    void Confirm()
    {
        if (m_isShowingResult) return;
        m_isCorrect = true;
        for (int i = 0; i < m_numOfDigits; i++)
        {
            if (m_numberChars[i] != m_currentData.m_targetValue[i])
            {
                m_isCorrect = false;
                break;
            }
        }
        ShowImage();
    }
}
