using System;
using System.Collections.Generic;
using UnityEngine;
public class ClimbAndJump_DataController : MonoBehaviour
{
    public static ClimbAndJump_DataController Instance;
    private static int m_currentMap;
    private static int m_spinsLeft;
  
    public int m_id;
    public AudioClip m_music;
    public bool UnlockFirstWing = false;
    public static int CurrentMap
    {
        get => m_currentMap;

        set
        {
            m_currentMap = value;
            IdleGameBase_PrefData.CurrentMap = value;
        }
    }
    
    public static int SpinsLeft
    {
        get => m_spinsLeft;

        set
        {
            m_spinsLeft = value;
            IdleGameBase_PrefData.SpinsLeft = value;
        }
    }
    public static bool EggTutorialDone
    {
        get=>IdleGameBase_PrefData.EggTutorial;
        set=>IdleGameBase_PrefData.EggTutorial = value;
    }
    private static int m_userOfflineTime;

    public SO_ShopData m_skinData;
    public static SkinController BloxSkinController = new SkinController();
    private void Awake()
    {
        if (Instance)
        {
            return;
        }
        Instance = this;

        DataController.InitMaxCoin();
       
        m_spinsLeft = IdleGameBase_PrefData.SpinsLeft;
        OfflineTime();
        SetMapUnlock(0);

        // InitWingData();
        BloxSkinController.m_id = PrefData.CurrentMinigame;
        BloxSkinController.m_skinData = m_skinData;
        BloxSkinController.InitSkinData();

        BloxSkinController.m_wingData = m_wingShopData;
        BloxSkinController.InitWingData();
        
        InitPetData();
        m_currentMap = IdleGameBase_PrefData.CurrentMap;
    }
    
    public void Initialise()
    {
        if (Instance == null)
        {
            Awake();
        }
    }
    
    private void OfflineTime()
    {
        string lastTimeStr = IdleGameBase_PrefData.User_LastTime;
        if (!string.IsNullOrEmpty(lastTimeStr))
        {
            DateTime lastTime = DateTime.Parse(lastTimeStr);
            TimeSpan offlineTime = DateTime.Now - lastTime;

            m_userOfflineTime = Mathf.Min((int)offlineTime.TotalMinutes, 24 * 60 * 5);

            //this.Log($"You offline {userOfflineTime} minutes");
        }
        SpinsLeft += m_userOfflineTime / (24 * 60);
    }
    
    private void OnDisable()
    {
        string lastTimeStr = IdleGameBase_PrefData.User_LastTime;
        if (!string.IsNullOrEmpty(lastTimeStr))
        {
            DateTime lastTime = DateTime.Parse(lastTimeStr);
            TimeSpan offlineTime = DateTime.Now - lastTime;

            m_userOfflineTime = Mathf.Min((int)offlineTime.TotalMinutes, 24 * 60 * 5);

            //this.Log($"You offline {userOfflineTime} minutes");
        }
        int g = m_userOfflineTime / (24 * 60);
        if (g == 0) return;
        IdleGameBase_PrefData.User_LastTime = DateTime.Now.ToString();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        string lastTimeStr = IdleGameBase_PrefData.User_LastTime;
        if (!string.IsNullOrEmpty(lastTimeStr))
        {
            DateTime lastTime = DateTime.Parse(lastTimeStr);
            TimeSpan offlineTime = DateTime.Now - lastTime;

            m_userOfflineTime = Mathf.Min((int)offlineTime.TotalMinutes, 24 * 60 * 5);

            //this.Log($"You offline {userOfflineTime} minutes");
        }
        int g = m_userOfflineTime / (24 * 60);
        if (g == 0) return;
        IdleGameBase_PrefData.User_LastTime = DateTime.Now.ToString();
    }
    public bool IsMapUnlocked(int id)
    {
        return IdleGameBase_PrefData.IsMapUnlocked(id);
    }
    public void SetMapUnlock(int id)
    {
        IdleGameBase_PrefData.SetMapUnlocked(id, true);
    }
    
    #region WINGS
    public SO_WingShopData m_wingShopData;
   
    public bool IsWingUnlocked(int id)
    {
        return BloxSkinController.IsWingUnlocked(id);
    }
    
    public static int UserCurrentWing
    {
        get => BloxSkinController.UserCurrentWing;
        set
        {
            BloxSkinController.UserCurrentWing = value;
            //IdleGameBase_PrefData.UserCurrentWing = value;
        }
    }
    private static string _userWingList;

    public void SetWingUnlock(int index)
    {
        BloxSkinController.SetWingUnlock(index);
    }

    #endregion
    
    #region PETS
    public SO_PetItemDataV2 m_petData;
    public int[,] m_petSave = new int[40, 3]; //map, egg, pet, level
    public SO_Rarity m_rarityData;
    private static string m_petUnlockState;
    public static string PetUnlockState
    {
        get => m_petUnlockState;
        set
        {
            m_petUnlockState = value;
            IdleGameBase_PrefData.PetUnlockState = value;
        }
    }
    public int GetPet(int id, int level)
    {
        return m_petSave[id, level];
    }
    public void SetPet(PetItem pet, int level, int amount)
    {
        SetPet(Array.IndexOf(m_petData.m_petItems, pet), level, amount);
    }
    public void SetPet(int id, int level, int amount)
    {
        m_petSave[id, level] = amount;
        IdleGameBase_PrefData.SetPetAmount(id, level, amount);
    }
    public void SetPetUnlock(int id, bool state)
    {
        if (IsPetUnlock(id) == state) return;
        PetUnlockState = IdleGameBase_PrefData.Replace(m_petUnlockState, id, 1, state ? "1" : "0");
    }
    public bool IsPetUnlock(int id)
    {
        return m_petUnlockState[id] == '1';
    }
    void InitPetData()
    {
        for (int k = 0; k < 40; k++)
        {
            for (int l = 0; l < 3; l++)
            {
                m_petSave[k, l] = IdleGameBase_PrefData.GetPetAmount(k, l);
            }
        }
        m_petUnlockState = IdleGameBase_PrefData.PetUnlockState;
    }
    #endregion
    
}
