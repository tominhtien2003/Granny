using Cysharp.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallStats : MonoBehaviour
{
    public TMP_Text m_id;
    public TMP_Text m_hp;
    public TMP_Text m_maxHp;
    public Image m_fill;
    public Sprite[] m_crackSprites;
    public Image m_crackImage;
    public Action<int> OnIdSet;
    public Action<int> OnBroken;
    public Action<int> OnHit;
    public void SetId(int id)
    {
        m_id.SetTextFormat("{0}", id + 1);
        OnIdSet?.Invoke(id);
    }
    public void SetHp(double hp, double maxHp)
    {
        MoneyUIView.UpdateMoney(maxHp , m_maxHp);
        MoneyUIView.UpdateMoney(hp, m_hp);

        m_fill.fillAmount = (float)hp / (float)maxHp;
        if (m_fill.fillAmount > .92f) {
            m_crackImage.enabled = false;
            return;
        }
        float slotAmount = (float)(maxHp / 3);
        int index = Mathf.FloorToInt((float)(hp / slotAmount));
        if (index < 0) index = 0;

        m_crackImage.enabled = true;
        m_crackImage.sprite = m_crackSprites[index];

    }
}
