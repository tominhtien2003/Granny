using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleAI_SkinDataSetter
{
    public ProgressBarData m_progressBar;
    public List<SkinShopData> m_skins;
    private SimpleAIScript[] m_ais;
    public Action<int, SkinShopData> OnSkinAssignedToBot;

    public SkinController BloxWingController;

    public SimpleAI_SkinDataSetter(ProgressBarData progressBar, List<SkinShopData> skins)
    {
        m_progressBar = progressBar;
        m_skins = skins;
    }
    public void Init()
    {
        m_ais = GameObject.FindObjectsOfType<SimpleAIScript>(true);
        BloxWingController = ClimbAndJump_DataController.BloxSkinController;
    }
    public void InitBotSkins()
    {
        for (int i = 0; i < m_ais.Length; i++)
        {
            SkinShopData shopData = m_skins[Random.Range(0, m_skins.Count)];
            SkinAssignUtils.AssignModel(m_ais[i].GetComponentInChildren<SkinnedMeshRenderer>(), shopData);
            m_ais[i].SetModel(shopData);
            if (m_progressBar != null) m_progressBar.m_raceImages[i].sprite = shopData.m_raceIcon;

            //OnSkinAssignedToBot?.Invoke(i, shopData);
        }
        List<WingDisplayData> wings = BloxWingController.m_wingData.m_wingShopItemData;
        for (int i = 0; i < m_ais.Length; i++)
        {
            WingDisplayData wingData = wings[Random.Range(0, wings.Count)];
            AssignPlayerModel(m_ais[i].m_wings, wingData);
        }
    }

    public virtual void AssignPlayerModel(WingsContainer wingsContainer, WingDisplayData shopData)
    {
        if (shopData == null || BloxWingController.m_skinData.m_shopItemData[BloxWingController.UserCurrentSkin].m_wingAvailable)
        {
            foreach (var w in wingsContainer.m_wingsRenderers) w.enabled = false;
            return;
        }
        foreach (var wing in wingsContainer.m_filters)
        {
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_mesh);
        }
        foreach (var wing in wingsContainer.m_wingsRenderers)
        {
            wing.enabled = true;
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_materials);
        }
        if (wingsContainer.m_stats != null) wingsContainer.m_stats.SetSpeed(shopData.m_climbSpeed);
    }

}
