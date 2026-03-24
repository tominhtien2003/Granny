
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCheat : MonoBehaviour
{
    #if CHEAT
    public TMP_InputField m_field;
    public MoneyController m_controller;
    private void Start()
    {
        GetComponent<ButtonEffectLogic>().onClick.AddListener(SetCurrency);
    }

    public void SetController(MoneyController moneyController)
    {
        m_controller = moneyController;
    }
    
    void SetCurrency()
    {
        if(m_controller) m_controller.CheatMoney(m_field.text);
    }
    #endif
}

