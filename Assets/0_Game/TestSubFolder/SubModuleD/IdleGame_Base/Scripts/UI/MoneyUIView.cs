using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;

public class MoneyUIView : MonoBehaviour
{
    public TMP_Text m_jumpCoinTxt;
    public TMP_Text m_totalCoinTxt;
    public TMP_Text m_multiplierTxt;
    public GameObject m_jumpCoinObject;
    public static void UpdateMoney(double money, TMP_Text text, bool formatInt = true)
    {
        if (money < 1000)
        {
            if (formatInt || !Utils.HasOneAfterDecimal(money)) text.SetTextFormat("{0}", (int)money);
            else text.SetTextFormat("{0:F1}", money);
        }
        else
        {
            double div = Utils.GetNumberAroundString(money, out int suffix);
            if (Utils.HasOneAfterDecimal(div))
            {
                text.SetTextFormat("{0:F1}{1}", div, Utils.suffixes[suffix]);
            }
            else text.SetTextFormat("{0}{1}", (int)div, Utils.suffixes[suffix]);
        }
    }
    public static void UpdateMoneyFormat(double money, TMP_Text text, bool formatInt = true, string format = "{0}")
    {
        double div = Utils.GetNumberAroundString(money, out int suffix);
        text.SetTextFormat(format, div, Utils.suffixes[suffix]);
    }
    
    public static void UpdateMoney(double current, double max, TMP_Text text, bool formatInt = true)
    {
        string left = FormatSingleMoney(current, formatInt);
        string right = FormatSingleMoney(max, formatInt);

        text.SetText($"{left}/{right}");
    }
    
    private static string FormatSingleMoney(double money, bool formatInt)
    {
        if (money < 1000)
        {
            if (formatInt || money % 1d == 0d)
                return ((long)money).ToString();

            // truncate 1 chữ số thập phân
            double v = Math.Truncate(money * 10d) * 0.1d;
            return v.ToString("0.0");
        }
        else
        {
            double div = Utils.GetNumberAroundString(money, out int suffix);
            if (div % 1d == 0d)
                return ((long)div) + Utils.suffixes[suffix];

            double value = Math.Truncate(div * 10d) * 0.1d;
            return value.ToString("0.0") + Utils.suffixes[suffix];
        }
    }

   
    public void OnUpdateTotalMoney(double money)
    {
        UpdateMoney(money, m_totalCoinTxt);
    }
    public void OnUpdateJumpMoney(double money)
    {
        if (money <= 0)
        {
            if (m_jumpCoinObject.activeSelf) m_jumpCoinObject.SetActive(false);
            return;
        }
        if (!m_jumpCoinObject.activeSelf) m_jumpCoinObject.SetActive(true);
        UpdateMoney(money, m_jumpCoinTxt);
        
    }

    public void OnUpdateMultiplier(double multiplier)
    {
        m_multiplierTxt.text  = ZString.Format("x{0:F1}", multiplier);
    }
}
