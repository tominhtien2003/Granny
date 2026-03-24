using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Adventure_GameManager : MonoBehaviour
{
    private static Adventure_GameManager _instance;
    public static Adventure_GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Adventure_GameManager>();
            }
            return _instance;
        }
    }

    public CheckpointManager cpManager;

    public SkinPurchasableManager m_skinsManager;
    public List<Transform> m_skinPos;
    public List<int> m_skinIds;
    public Bot_SkinDataSetter _botSkinDataSetter;

    public PlayerBloxBrain playerBloxBrain;
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    protected virtual void Start()
    {
        cpManager.OnPlayerRespawnEvent += OnPlayerRespawn;
        cpManager.OnPlayerDeathEvent += OnPlayerDeath;
        _botSkinDataSetter.InitBotSkins();

        SkinPurchasableZone[] zones4 = m_skinsManager.GetComponentsInChildren<SkinPurchasableZone>(true);
        for (int i = 0; i < zones4.Length; i++) m_skinPos[i].position = zones4[i].transform.position;
        int slotCount = m_skinPos.Count;

        m_skinIds = Utils.GetSkinsForMap(slotCount);

        if (m_skinIds.Count == 0) return;
        ResetSkins();
        
    }
    protected virtual void OnPlayerRespawn()
    {
        
    }

    public Joystick joystick;
    public ParticleSystem carExplosion;
    protected virtual void OnPlayerDeath()
    {
        AudioManager.Instance.PlayDeadSound();
        if (joystick != null)
        {
            joystick.ResetJoystick();
        }
        if (Adventure_InventoryManager.Instance != null)
        {
            Adventure_InventoryManager.Instance.OnPlayerDied();
        }
    }
    void ResetSkins()
    {
        m_skinsManager.SetSkinPos(m_skinPos);
        m_skinsManager.SetSkinsIds(m_skinIds);
    }
    public List<int> GetSkinsForMap(int slotCount)
    {
        var skinController = DataController.BloxSkinController;
        var skins = skinController.m_skinData.m_shopItemData;

        List<int> unowned = new List<int>();

        for (int id = 0; id < skins.Count; id++)
        {
            if (!skinController.IsSkinUnlocked(id))
                unowned.Add(id);
        }

        if (unowned.Count == 0)
            return new List<int>();

        List<int> result = new List<int>();

        for (int i = 0; i < slotCount; i++)
        {
            int randomId = unowned[Random.Range(0, unowned.Count)];
            result.Add(randomId);
        }

        return result;
    }

    public void TeleportCheat(int id)
    {
        if (id > cpManager.Checkpoints.Length - 1 || id < 0) return;

        cpManager.TeleportPlayerToCheckpoint(id);
    }
    protected virtual void OnDestroy()
    {
        cpManager.OnPlayerRespawnEvent -= OnPlayerRespawn;
        cpManager.OnPlayerDeathEvent -= OnPlayerDeath;
    }
}
