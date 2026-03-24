using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuButtons : MonoBehaviour
{
    public ButtonEffectLogic m_skin, m_shop, m_inventory, m_rewards;
    public BasePopup m_skinPopup, m_shopPopup, m_inventoryPopup, m_rewardsPopup;
    private void Awake()
    {
        if(m_skin && m_skinPopup) m_skin.onClick.AddListener(m_skinPopup.Show);
        if(m_shop) m_shop.onClick.AddListener(m_shopPopup.Show);
        if(m_inventory && m_inventoryPopup != null) m_inventory.onClick.AddListener(m_inventoryPopup.Show);
        if(m_rewards) m_rewards.onClick.AddListener(m_rewardsPopup.Show);
    }
}
