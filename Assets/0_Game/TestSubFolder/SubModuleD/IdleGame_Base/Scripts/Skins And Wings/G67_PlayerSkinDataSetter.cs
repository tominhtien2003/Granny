using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G67_PlayerSkinDataSetter : PlayerSkinDataSetter
{
    public Image m_playerIcon;
    public override void AssignPlayerModel(SkinShopData shopData)
    {
        SkinAssignUtils.AssignModel(m_playerMesh, shopData);
        m_playerIcon.sprite = shopData.m_raceIcon;
    }
}
