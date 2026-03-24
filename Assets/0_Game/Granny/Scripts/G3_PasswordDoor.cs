using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class G3_PasswordDoor : Adventure_InteractObj
{
    public CalculatorData m_data;
    [SerializeField] TextMeshProUGUI[] digitTexts;
    protected override void Awake()
    {
        base.Awake();
        m_data.m_targetValue = Random.Range(0, 10000).ToString("D4");
        for (int i = 0; i < digitTexts.Length; i++)
        {
            digitTexts[i].text = m_data.m_targetValue[i].ToString();
        }
    }
    protected override void Start()
    {
        base.Start();
        G3_UIManager.Instance.calculatorPopup.InitialiseUI();
    }
    public override void Interact()
    {
        base.Interact();
        G3_UIManager.Instance.calculatorPopup.Show();
        G3_UIManager.Instance.calculatorPopup.InitPopup(m_data);
    }
}
