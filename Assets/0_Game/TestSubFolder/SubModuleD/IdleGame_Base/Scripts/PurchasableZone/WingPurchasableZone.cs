using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WingPurchasableZone : BaseAdZone
{
    public int m_wingIndex;
    public override int ItemId { get => m_wingIndex; set => m_wingIndex = value; }
    public UnityAction<IAdAmount> OnTryUnlock;

    public MeshRenderer m_renderer;
    public MeshFilter m_meshFilter;
    public void InitWingZone()
    {
        ClimbAndJump_DataController controller = ClimbAndJump_DataController.Instance;
        if (controller.IsWingUnlocked(m_wingIndex)) gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            SO_WingsData wdt = controller.m_wingShopData.m_wingShopItemData[ItemId].m_wingsData;
           
            m_renderer.sharedMaterials = wdt.m_materials.ToArray();
            m_meshFilter.sharedMesh = wdt.m_mesh;
            ClimbAndJump_DataController.BloxSkinController.m_wingDataHolder.AddItem(this, true);
            SetRing(Mathf.Clamp(m_wingIndex / 15, 0 , 1));
        }
        
    }
    public void SetId(int id)
    {
        m_wingIndex = id;
        InitWingZone();
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
