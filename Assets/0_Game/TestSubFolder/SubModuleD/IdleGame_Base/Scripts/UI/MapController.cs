using Cinemachine;
using KinematicCharacterController;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public KinematicCharacterMotor m_player;
    public SimpleAIScript[] m_ais;

    public SphereVisualizer m_sphereVis;

    public Transform m_playerResetPos;
    public MapData_SO m_mapData;
    TeleportLocation m_currentLocation;
    private ISpeedChanger m_trussClimb;
    public EggsManager m_eggManager;
    public WingPurchasableManager m_wingsManager;
    public PetPurchasableManager m_petManager;
    public SkinPurchasableManager m_skinManager;
    public CupScript m_cup;
    public HeightMoneyController m_moneyController;

    public AIPetSetter m_petSetter;
    public CinemachineVirtualCamera m_cam;
    public IResetable[] m_resetables;
    private void Awake()
    {
        m_ais = FindObjectsOfType<SimpleAIScript>();
        m_trussClimb = m_player.GetComponentInChildren<ISpeedChanger>();
        m_resetables = m_wingsManager.transform.parent.GetComponentsInChildren<IResetable>(true);
        
        //TrackingEvent.LogStartLv(GameManager.MiniGameIndex);
    }
    void Start()
    {

        //TrackingEvent.LogStartLv(GameManager.MiniGameIndex);
    }
    public void InitSwapMap(TeleportLocation location)
    {
        m_botSkin = new SimpleAI_SkinDataSetter(PlayerSkinDataSetter.Instance.m_progressBar, PlayerSkinDataSetter.Instance.BloxSkinController.m_skinData.m_shopItemData);
        m_botSkin.Init();
        m_petSetter.StartInit();
        ResetAll(location);
    }
    void ResetAll(TeleportLocation location)
    {
        location.m_locationGameObject.SetActive(true);
        m_currentLocation = location;
        m_cup.transform.position = location.m_cupPos.position;
        m_cup.SetCupMultiplier(location.m_cupMultiplier);
        m_moneyController.m_mapMultiplier = location.m_moneyMultiplier;
        ResetPlayer();
        ResetEggs();
        ResetPets();
        ResetBots();
        ResetWings();
        ResetSkins();
        if (PunchWall_GlobalStatusHolder.Instance != null)
        {
            PunchWall_GlobalStatusHolder.WallMultiplier = location.m_climbMultiplier;
            PunchWall_GlobalStatusHolder.Instance.ResetWall();


        }
        foreach (var resetable in m_resetables)
        {
            resetable.ResetInit();
        }
    }
    public void SwapMap(TeleportLocation location)
    {
        if (m_currentLocation == location) return;
        

        if (m_currentLocation != null) m_currentLocation.m_locationGameObject.SetActive(false);
        ResetAll(location);
    }
    public void ResetPlayer()
    {
        m_player.SetPosition(m_playerResetPos.position);
        m_player.SetRotation(Quaternion.Euler(0, -90, 0));
        m_cam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = -90;
        m_trussClimb?.SetCurrentGlobalMultiplier(m_currentLocation.m_climbMultiplier);
    }
    public void SetBonusClimb(float mult = 1f)
    {
        m_trussClimb?.SetCurrentGlobalMultiplier(m_currentLocation.m_climbMultiplier * mult);
    }
    public SimpleAI_SkinDataSetter m_botSkin;
    public void ResetBots()
    {
        for (int i = 0; i < m_ais.Length; i++)
        {
            SimpleAIScript ai = m_ais[i];
            Vector3 pos = Random.insideUnitSphere * m_sphereVis.Radius + m_sphereVis.transform.position;
            pos.y = .1f;

            ai.Teleport(pos);


            ai.ResetBot();
            ai.SetCurrentGlobalMultiplier(m_currentLocation.m_climbMultiplier);
        }
        m_botSkin.InitBotSkins();
        PlayerWingDataSetter.Instance.InitBotWings();
        m_petSetter.AssignAIPets();
    }

    void ResetEggs()
    {
        m_eggManager.SetEggsData(m_currentLocation.m_eggsData);
        m_eggManager.SetEggPos(m_currentLocation.m_posHolder.m_eggPos);

    }
    void ResetPets()
    {
        m_petManager.SetPetPos(m_currentLocation.m_posHolder.m_petPos);
        m_petManager.SetPetIds(m_currentLocation.m_posHolder.m_petIds);
    }
    void ResetWings()
    {
        m_wingsManager.SetWingPos(m_currentLocation.m_posHolder.m_wingPos);
        m_wingsManager.SetWingsIds(m_currentLocation.m_posHolder.m_wingsIds);
    }
    void ResetSkins()
    {
        m_skinManager.SetSkinPos(m_currentLocation.m_posHolder.m_skinPos);
        m_skinManager.SetSkinsIds(m_currentLocation.m_posHolder.m_skinIds);
    }
    void SetShopPos()
    {
        
    }
}
