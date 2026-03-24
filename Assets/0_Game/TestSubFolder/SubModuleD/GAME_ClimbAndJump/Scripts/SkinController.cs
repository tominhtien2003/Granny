using System.Collections.Generic;
using UnityEngine;

public class SkinController
{
    public SkinItemDataHolder m_skinDataHolder;
    public SO_ShopData m_skinData;
    public int m_id = -1;
    
    private Dictionary<ShopItem, int> m_skinDict = new();
    private int _userCurSkin;
    public void InitSkinData()
    {
        _userCurSkin = IdleGameBase_PrefData.UserCurrentSkin(m_id);
        _userSkinList = IdleGameBase_PrefData.UserGetSkinList(m_id);
        

        m_skinDataHolder = new SkinItemDataHolder();
        m_skinDataHolder.BloxSkinController = this;
        m_skinDict = new();
        for (int i = 0; i < m_skinData.m_shopItemData.Count; i++)
        {
            ShopItem skin = m_skinData.m_shopItemData[i];
            m_skinDataHolder.AddInit(new SkinItemAdStats(0, skin.m_itemAdAmount));
            m_skinDict.Add(skin, i);
        }
        SetSkinUnlock(0);
    }
    
    public bool IsSkinSelected(ShopItem skin)
    {
        return m_skinData.m_shopItemData[_userCurSkin] == skin;
    }
    public bool IsSkinUnlocked(ShopItem skin)
    {
        return _userSkinList[m_skinDict[skin]] == '1';
    }
    public bool IsSkinUnlocked(int id)
    {
        return _userSkinList[id] == '1';
    }
    public void AssignSkinAdAmount(IAdAmount skin, bool useMoney = false)
    {
        m_skinDataHolder.AddItem(skin, useMoney);

    }
    public void SetCurrentSkin(ShopItem skin)
    {
        UserCurrentSkin = m_skinDict[skin];
    }
    public int UserCurrentSkin
    {
        get => _userCurSkin;
        set
        {
            _userCurSkin = value;
            IdleGameBase_PrefData.UserSetCurrentSkin(value, m_id);
        }
    }
    private string _userSkinList;

    private string UserSkinList
    {
        get => _userSkinList;
        set
        {
            _userSkinList = value;
            IdleGameBase_PrefData.UserSetSkinList(_userSkinList, m_id);
        }
    }

    public void SetSkinUnlock(int index)
    {
        string temp = IdleGameBase_PrefData.Replace(_userSkinList, index, 1, "1");
        UserSkinList = temp;
    }
    
    [Header("Wing")]
    public WingItemDataHolder m_wingDataHolder;
    public SO_WingShopData m_wingData;
    public int m_id_wing = -1;
    private Dictionary<ShopItem, int> m_wingDict = new();
    private int _userCurWing;
    private string _userWingList;
    public void InitWingData()
    {
        _userCurWing = IdleGameBase_PrefData.UserCurrentWing(m_id_wing);
        _userWingList = IdleGameBase_PrefData.UserGetWingList(m_id_wing);
        

        m_wingDataHolder = new WingItemDataHolder();
        m_wingDataHolder.BloxSkinController = this;
        m_wingDict = new();
        for (int i = 0; i < m_wingData.m_wingShopItemData.Count; i++)
        {
            ShopItem skin =  m_wingData.m_wingShopItemData[i];
            m_wingDataHolder.AddInit(new SkinItemAdStats(0, skin.m_itemAdAmount));
            m_wingDict.Add(skin, i);
        }
    }
    public void SetWingUnlock(int index)
    {
        string temp = IdleGameBase_PrefData.Replace(_userWingList, index, 1, "1");
        _userWingList = temp;
        IdleGameBase_PrefData.UserSetWingList(_userWingList, m_id_wing);
    }
    
    public int UserCurrentWing
    {
        get => _userCurWing;
        set
        {
            _userCurWing = value;
            IdleGameBase_PrefData.UserSetCurrentWing(value, m_id_wing);
        }
    }
    public bool IsWingSelected(ShopItem skin)
    {
        return m_wingDict[skin] == _userCurWing;
    }
    public bool IsWingUnlocked(ShopItem skin)
    {
        return _userWingList[m_wingDict[skin]] == '1';
    }
    public bool IsWingUnlocked(int id)
    {
        return _userWingList[id] == '1';
    }
    public void AssignWingAdAmount(IAdAmount skin, bool useMoney = false)
    {
        m_wingDataHolder.AddItem(skin, useMoney);

    }
    public void SetCurrentWing(ShopItem skin)
    {
        UserCurrentWing = m_wingDict[skin];
    }
}
