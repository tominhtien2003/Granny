using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPopup : BasePopup, IInitable
{
    public List<TeleportLocation> m_teleportLocations;
    public MapData_SO m_mapData;
    public TeleportItemButtonAddon m_teleportButtonPrefab;
    public Transform m_contentTransform;
    public MapController m_mapController;
    public MoneyController m_cupMoney;
    public Image m_towerImage;
    public void InitPopup()
    {
        //m_teleportLocations = m_mapData.m_locations;
        if (m_mapController == null) m_mapController = FindAnyObjectByType<MapController>();
        if (m_teleportLocations.Count == 0) return;
        m_mapController.InitSwapMap(m_teleportLocations[ClimbAndJump_DataController.CurrentMap]);
        InitButtons();
    }
    public void InitButtons()
    {
        for (int i = 0; i < m_teleportLocations.Count; i++)
        {
            TeleportItemButtonAddon button = Instantiate(m_teleportButtonPrefab, m_contentTransform);
            button.Init(m_teleportLocations[i]);
            if (ClimbAndJump_DataController.Instance.IsMapUnlocked(i)) {
                m_teleportLocations[i].m_isUnlocked = true;
                button.SetUnlockedState();
            }
            button.OnClick += TeleportTo;
        }
    }
    void TeleportTo(TeleportItemButtonAddon button)
    {
        var location = button.m_locationData;
        if (!location.m_isUnlocked)
        {
            if (m_cupMoney.AttemptPurchase(location.m_cupsRequired))
            {
                //TrackingEvent.LogFirebase($"climb_unlock_map_{ClimbAndJump_DataController.CurrentMap}", null);
                /*TrackingEvent.LogFirebase($"unlock_map_game_{GameManager.MiniGameIndex}", new Parameter[]
                {
                    new Parameter(Consts.MapID, ClimbAndJump_DataController.CurrentMap)
                });*/
                location.m_isUnlocked = true;
                ClimbAndJump_DataController.Instance.SetMapUnlock(m_teleportLocations.IndexOf(location));
            }
            button.SetUnlockedState();
           
            return;
        }
        //TrackingEvent.LogFirebase($"climb_teleport_map_{ClimbAndJump_DataController.CurrentMap}", null);
        /*TrackingEvent.LogFirebase($"teleport_map_game_{GameManager.MiniGameIndex}", new Parameter[]
        {
            new Parameter(Consts.MapID, ClimbAndJump_DataController.CurrentMap)
        });*/
        ClimbAndJump_DataController.CurrentMap = m_teleportLocations.IndexOf(location);
        m_mapController.SwapMap(location);
        if (m_towerImage != null) m_towerImage.sprite = location.m_towerSprite;
        Hide();
    }

    public void Init()
    {
        InitPopup();
    }
}
