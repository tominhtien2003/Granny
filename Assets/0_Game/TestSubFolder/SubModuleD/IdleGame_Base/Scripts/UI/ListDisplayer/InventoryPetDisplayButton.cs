using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryPetDisplayButton : ListDisplayItemButton
{
    public InventoryPetItem m_petItemData;
    public Text m_levelText;
    public TMP_Text m_multiplierText;

    public int m_currentIndex;
    public bool m_isEquipped = false;
    public UnityAction<InventoryPetDisplayButton> ButtonSelectedAction;
    public Image m_selectedImage;
    public Image m_selectedTick;
    public Image m_bg;

    //public Sprite m_empty;
    public bool m_isLocked;
    public Image m_lockedGraphic;
    public Image m_levelGraphic;
    public Sprite[] m_levelSprites;
    public void SwitchLockState()
    {
        m_isLocked = !m_isLocked;
        m_lockedGraphic.enabled = m_isLocked;
    }
    public override void InitButton(DisplayItem item)
    {
        base.InitButton(item);
        m_selectButton.onClick.AddListener(() => ButtonSelectedAction(this));
    }
    public void SetEquipGraphic()
    {
        m_selectedImage.enabled = m_isEquipped;
        m_selectedTick.enabled = m_isEquipped;
    }
    public void SetOrder(int index)
    {
        m_currentIndex = index;
        transform.SetSiblingIndex(index);
    }
    public override void SetButtonGraphic(DisplayItem item)
    {
        base.SetButtonGraphic(item);
        if (item == null)
        {
            gameObject.SetActive(false);
           m_petItemData = null;
            /*
            m_bg.sprite = m_empty;
            m_multiplierText.gameObject.SetActive(false);*/
            return;
        }
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        m_petItemData = item as InventoryPetItem;
        m_bg.sprite = ClimbAndJump_DataController.Instance.m_rarityData.m_rarityColors[m_petItemData.m_petItem.m_rarityIndex].m_buttonSprite;
        m_levelText.text = m_petItemData.m_level.ToString();
        
        SetInforMulti();

        m_levelGraphic.enabled = m_petItemData.m_level > 0;
        m_levelGraphic.sprite = m_levelSprites[m_petItemData.m_level];
    }

    protected virtual void SetInforMulti()
    {
        
        if (!m_multiplierText.gameObject.activeSelf)  m_multiplierText.gameObject.SetActive(true);
        MoneyUIView.UpdateMoney(m_petItemData.m_currentMultiplier, m_multiplierText, false);
    }
}
