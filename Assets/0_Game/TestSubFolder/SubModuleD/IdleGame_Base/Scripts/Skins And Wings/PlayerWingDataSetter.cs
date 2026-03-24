using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWingDataSetter : MonoBehaviour,ISkinControllerUser
{
    public WingsContainer m_playerWings;

    private SimpleAIScript[] m_ais;
    public UnityAction m_onWingAssigned;
    public static PlayerWingDataSetter Instance { get; private set; }
    public SkinController BloxSkinController { get; set; }
    public bool UnlockFirst = false;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        m_ais = FindObjectsOfType<SimpleAIScript>(true);
        
    }
    private void Start()
    {
        CheckSetWing();
    }

    private void CheckSetWing()
    {
        if (BloxSkinController.m_wingData)
        {
            m_playerWings.gameObject.SetActive(BloxSkinController.UserCurrentWing != -1);
            if (BloxSkinController.UserCurrentWing != -1)AssignPlayerModel(BloxSkinController.m_wingData.m_wingShopItemData[BloxSkinController.UserCurrentWing]);
            
        }
    }
    
    public void InitBotWings()
    {
        // m_wingsList = BloxSkinController.m_skinData.m_wingItemData;
        // if (m_wingsData == null) m_wingsData = BloxSkinController.m_skinData.m_wingItemData;
        //
        // for (int i = 0; i < 2; i++)
        // {
        //     if (m_ais[i].m_wings == null) continue;
        //
        //     AssignBotWings(m_ais[i], Random.Range(3, 7));
        // }
        // for (int i = 2; i < m_ais.Length; i++)
        // {
        //     if (m_ais[i].m_wings == null) continue;
        //      AssignBotWings(m_ais[i], Random.Range(m_wingsList.Count - 10, m_wingsList.Count - 1));
        //
        // }
    }

    //public void AssignPlayerWings(int id, bool setSpeed = false)
    //{
    //    if (id == -1)
    //    {
    //        AssignWings(m_playerWings, null);
    //        return;
    //    }
    //    var data = BloxSkinController.m_skinData.m_wingItemData[id];

    //    m_playerWings.m_id = id;
    //    m_onWingAssigned?.Invoke();

    //    AssignWings(m_playerWings, data);
    //    if (setSpeed)
    //    {
    //        m_playerWings.m_stats.SetSpeed(data.m_climbSpeed);
    //    }
    //}

    public void CheckActiveWing(bool active)
    {
        if (m_playerWings.m_wingsRenderers[0].enabled != active)
        {
            CheckSetWing();
        }
    }
    
    public virtual void AssignPlayerModel(WingDisplayData shopData)
    {
        if (shopData == null || BloxSkinController.m_skinData.m_shopItemData[BloxSkinController.UserCurrentSkin].m_wingAvailable)
        {
            foreach (var w in m_playerWings.m_wingsRenderers) w.enabled = false;
            return;
        }
        foreach (var wing in m_playerWings.m_filters)
        {
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_mesh);
        }
        foreach (var wing in m_playerWings.m_wingsRenderers)
        {
            wing.enabled = true;
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_materials);
        }
        if (m_playerWings.m_stats != null) m_playerWings.m_stats.SetSpeed(shopData.m_climbSpeed);
    }
    public virtual void AssignPlayerModel(WingSkinShopData wingData)
    {
        if (wingData == null)
        {
            foreach (var w in m_playerWings.m_wingsRenderers) w.enabled = false;
            return;
        }
        foreach (var wing in m_playerWings.m_filters)
        {
            SkinAssignUtils.AssignModel(wing, wingData.m_mesh);
        }
        foreach (var wing in m_playerWings.m_wingsRenderers)
        {
            wing.enabled = true;
            SkinAssignUtils.AssignModel(wing, wingData.m_materials);
        }
        
    }
    
    // public void AssignBotWings(SimpleAIScript botWings, int id, bool setSpeed = false)
    // {
    //     WingDisplayData data = m_wingsList[id];
    //     botWings.m_wings.m_id = id;
    //     AssignWings(botWings.m_wings, data);
    //     if (setSpeed)
    //     {
    //         botWings.SetSpeed(data.m_climbSpeed);
    //     }
    // }
    
    public void AssignWings(WingsContainer container, WingDisplayData data)
    {
        if (data == null)
        {
            foreach (var w in container.m_wingsRenderers) w.enabled = false;
            return;
        }
        else foreach (var w in container.m_wingsRenderers) w.enabled = true;

        foreach (var w in container.m_wingsRenderers)
        {
            Material[] sharedMaterials = new Material[data.m_wingsData.m_materials.Count];

            for (int i = 0; i < data.m_wingsData.m_materials.Count; i++)
            {
                sharedMaterials[i] = data.m_wingsData.m_materials[i];
            }
            w.sharedMaterials = sharedMaterials;
        }
        foreach (var f in container.m_filters) f.sharedMesh = data.m_wingsData.m_mesh;
        container.OnWingAssigned?.Invoke();
    }
}
