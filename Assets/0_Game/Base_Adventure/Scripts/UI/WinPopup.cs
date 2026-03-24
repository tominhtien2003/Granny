using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinPopup : BasePopup
{
    public Image characterImage;

    public void SetCharacterImage()
    {
        SkinController controller = DataController.BloxSkinController;
        characterImage.sprite = controller.m_skinData.m_shopItemData[controller.UserCurrentSkin].m_displayIcon;
    }
    public override void Show()
    {
        base.Show();
        ParticleManager.Instance.PlayConfetti();
        SetCharacterImage();
    }
}
