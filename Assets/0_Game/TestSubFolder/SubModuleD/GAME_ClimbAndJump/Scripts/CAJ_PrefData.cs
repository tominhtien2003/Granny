using Cysharp.Text;
using UnityEngine;
public abstract class IdleGameBase_PrefData
{
    
    public static string m_coin = "coins";
    public static string m_cup = "cups";
    public static string m_gems = "gems";
    public static string m_highestCoins = "highest_coins";
    public static double GetCoins(string defaultValue = "0")
    {
        return GetDouble(m_coin, PrefData.CurrentMinigame, defaultValue);
    }
    public static double GetCups()
    {
        return GetDouble(m_cup, PrefData.CurrentMinigame);
    }
    public static double GetGems()
    {
        return GetDouble(m_gems, PrefData.CurrentMinigame);
    }
    public static void SaveCoin(double amount)
    {
        SaveDouble(m_coin, PrefData.CurrentMinigame, amount);
    }
    public static void SaveCups(double amount)
    {
        SaveDouble(m_cup, PrefData.CurrentMinigame, amount);
    }
    public static void SaveGems(double amount)
    {
        SaveDouble(m_gems, PrefData.CurrentMinigame, amount);
    }
    public static double MaxCoins
    {
        get=> GetDouble(m_highestCoins, PrefData.CurrentMinigame);
        set=> SaveDouble(m_highestCoins, PrefData.CurrentMinigame, value);
    }
    public static double GetDouble(string currency, int minigame = 0, string defaultValue = "0")
    {
        string g = PlayerPrefs.GetString(ZString.Format("{0}_{1}", currency, minigame), defaultValue);
        double h = double.TryParse(g, out double result) ? result : 0;
        return h;
    }
    public static void SaveDouble(string currency, int minigame = 0, double amount = 0)
    {
        PlayerPrefs.SetString(ZString.Format("{0}_{1}", currency, minigame), ZString.Format("{0}", amount));
    }

    public static int GetInt(string key, int minigame = 0, int defaultValue = 0)
    {
        int g = PlayerPrefs.GetInt(ZString.Format("{0}_{1}", key, minigame), defaultValue);
        return g;
    }
    public static void SetInt(string key, int minigame = 0, int val = 0)
    {
        PlayerPrefs.SetInt(ZString.Format("{0}_{1}", key, minigame), val);
    }
    
    public static string GetString(string key, int minigame = 0, string defaultValue = "")
    {
        string g = PlayerPrefs.GetString(ZString.Format("{0}_{1}", key, minigame), defaultValue);
        return g;
    }
    public static void SetString(string key, int minigame = 0, string val = "")
    {
        PlayerPrefs.SetString(ZString.Format("{0}_{1}", key, minigame), val);
    }
    
    public static int RewardTime
    {
        get => GetInt("reward_time", PrefData.CurrentMinigame, 3600);
        set => SetInt("reward_time", PrefData.CurrentMinigame, value);
    }

    public static string RewardState
    {
        get => GetItemList("reward_state", PrefData.CurrentMinigame);
        set => SetItemList("reward_state", PrefData.CurrentMinigame,value);
    }
    public static string PetUnlockState
    {
        get=> GetItemList("pet_unlock_state", PrefData.CurrentMinigame);
        set=> SetItemList("pet_unlock_state", PrefData.CurrentMinigame, value);
       
    }
    public static bool EggTutorial
    {
        get => PlayerPrefs.GetInt(ZString.Format("egg_tutorial_{0}", PrefData.CurrentMinigame), 0) == 1;
        set => PlayerPrefs.SetInt(ZString.Format("egg_tutorial_{0}", PrefData.CurrentMinigame), value ? 1 : 0);
    }
    public static int SpinsLeft
    {
        get => PlayerPrefs.GetInt(ZString.Format("spins_{0}", PrefData.CurrentMinigame), 1);
        set => PlayerPrefs.SetInt(ZString.Format("spins_{0}", PrefData.CurrentMinigame), value);
    }
    public static bool IsMapUnlocked(int i)
    {
        return GetInt(ZString.Format("map_{0}", i), PrefData.CurrentMinigame, 0) != 0;
    }

    public static void SetMapUnlocked(int i, bool value)
    {
        SetInt("map_" + i, PrefData.CurrentMinigame, value ? 1 : 0);
    }
    public static int CurrentMap
    {
        get=> GetInt("current_map", PrefData.CurrentMinigame, 0);
        set=> SetInt("current_map", PrefData.CurrentMinigame, value);
    }
    #region WINGS
    // public static int UserCurrentWing
    // {
    //     get => GetInt("user_current_wing", PrefData.CurrentMinigame , -1);
    //     set => SetInt("user_current_wing", PrefData.CurrentMinigame, value);
    // }
    
    public static int UserCurrentWing(int minigameId = -1)
    {
        return GetInt("user_current_wing", minigameId, -1);
        
    }
    public static void UserSetCurrentWing(int value, int minigameId = -1)
    {
        SetInt("user_current_wing", minigameId, value);
    }
    
    static string GetStringList(string key, int amount = 50)
    {
        string h = PlayerPrefs.GetString(key, "");
        if (h.Length >= 1) return h;
        else
        {
            string g = "";
            for (int i = 0; i < amount; i++) g += "0";
            PlayerPrefs.SetString(key, g);
            return g;
        }
    }
    public static string GetItemList(string key, int minigame, int amount = 65)
    {
        return GetStringList(ZString.Format("{0}_{1}", key, minigame), amount);
    }
    public static void SetStringList(string key, string list)
    {
        PlayerPrefs.SetString(key, list);
    }
    public static string User_LastTime
    {
        get => PlayerPrefs.GetString(ZString.Format("user_last_time_{0}",PrefData.CurrentMinigame), "");
        set => SetItemList("user_last_time",PrefData.CurrentMinigame, value);
    }
    public static void SetItemList(string key, int minigame, string list)
    {
        SetStringList(ZString.Format("{0}_{1}",key, minigame), list);
    }
    public static string UserGetWingList(int minigame = 0)
    {
        return GetItemList("user_wing_list", PrefData.CurrentMinigame);
    }

    public static void UserSetWingList(string skinList, int minigame = 0)
    {
        SetItemList("user_wing_list", PrefData.CurrentMinigame, skinList);
    }
    #endregion
    
    #region SKINS
    public static int UserCurrentSkin(int minigameId = -1)
    {
        return GetInt("user_current_skin", minigameId, 0);
        
    }
    public static void UserSetCurrentSkin(int value, int minigameId = -1)
    {
        SetInt("user_current_skin", minigameId, value);
    }
    public static string UserGetSkinList(int minigameId = -1)
    {
        return GetItemList("user_skin_list", minigameId);
    }

    public static void UserSetSkinList(string skinList, int minigameId = -1)
    {
        SetItemList("user_skin_list", minigameId, skinList);
    }
    #endregion
    #region PETS
    public static int GetPetAmount(int petId, int level)
    {
        string key = ZString.Format("pet_amount_{0}_{1}_{2}", petId, level, PrefData.CurrentMinigame);
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void SetPetAmount(int petId, int level, int amount)
    {
        string key = ZString.Format("pet_amount_{0}_{1}_{2}", petId, level, PrefData.CurrentMinigame);
        PlayerPrefs.SetInt(key, amount);
    }
    #endregion
    
    public static string Replace(string text, int start, int count,
                             string replacement)
    {
        //Debug.Log("replacement is " + replacement + " " + text.Length);
        if (start >= text.Length)
        {
            ZString.Concat(text, new string('0', start - text.Length + 1));
        }
        return ZString.Concat(text.Substring(0, start), replacement, text.Substring(start + count));
    }
}