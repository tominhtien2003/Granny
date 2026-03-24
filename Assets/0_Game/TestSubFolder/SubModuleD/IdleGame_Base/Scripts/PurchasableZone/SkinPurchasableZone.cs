using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkinPurchasableZone : BaseAdZone
{
    public int m_skinIndex;
    public override int ItemId { get => m_skinIndex; set => m_skinIndex = value; }
    public UnityAction<SkinPurchasableZone> OnTryUnlock;

    public int m_currentCnt;

    public SkinnedMeshRenderer m_renderer;
    public MeshRenderer[] m_subRenderers;
    public SkinController BloxSkinController;

    public void InitSkinZone()
    {
        if (BloxSkinController.IsSkinUnlocked(m_skinIndex)) gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            SkinShopData wdt = BloxSkinController.m_skinData.m_shopItemData[m_skinIndex];
            Material[] sharedMaterials = new Material[wdt.m_materials.Count];

            for (int i = 0; i < wdt.m_materials.Count; i++)
            {
                sharedMaterials[i] = wdt.m_materials[i];
            }
            
            m_renderer.sharedMaterials = sharedMaterials;
            m_renderer.sharedMesh = wdt.m_mesh;

            foreach (var sub in m_subRenderers)
            {
                sharedMaterials = sub.sharedMaterials;
                sharedMaterials[0] = wdt.m_materials[0];
                sub.sharedMaterials = sharedMaterials;
            }

            BloxSkinController.m_skinDataHolder.AddItem(this, wdt.m_itemPrice > 0);
            SetRing(Mathf.Clamp(m_skinIndex / 15, 0 , 1));
        }
    }
    
    #if UNITY_EDITOR

    public void Init(SkinShopData skinShopData)
    {
        gameObject.name = skinShopData.m_itemName;
        SkinShopData wdt = skinShopData;
        Material[] sharedMaterials = new Material[wdt.m_materials.Count];

        for (int i = 0; i < wdt.m_materials.Count; i++)
        {
            sharedMaterials[i] = wdt.m_materials[i];
        }
        m_renderer.sharedMaterials = sharedMaterials;
        m_renderer.sharedMesh = wdt.m_mesh;
    }
    [Button]
    public void InitSubs()
    {
        m_subRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    
    #endif
    
    public int m_maxCnt;
    public override void OnUpdateAdAmount(int amount, int maxAmount)
    {
        base.OnUpdateAdAmount(amount, maxAmount);
        m_currentCnt = amount;
        m_maxCnt = maxAmount;
    }

    public void SetId(int id)
    {
        OnDestroyed?.Invoke(this);
        m_skinIndex = id;
        InitSkinZone();
    }
    protected override void TryUnlock()
    {
        OnTryUnlock?.Invoke(this);
    }
    protected override void Unlocked()
    {
        base.Unlocked();
        gameObject.SetActive(false);
    }
}
