using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMultiplierInjector : MonoBehaviour
{
    public HeightMoneyController m_moneyController;

    public void Awake()
    {
        FindObjectOfType<InventoryListDisplayPopup>(true).m_petMultiplySetter = m_moneyController;
        Destroy(this);
    }

}

