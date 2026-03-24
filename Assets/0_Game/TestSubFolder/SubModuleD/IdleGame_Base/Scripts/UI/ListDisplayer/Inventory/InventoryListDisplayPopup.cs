using Cysharp.Text;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct PetItemStruct
{
    public PetItemStruct(PetItem petItem, int level)
    {
        m_petItem = petItem;
        m_level = level;
    }
    public PetItem m_petItem;
    public int m_level;
}
public class InventoryListDisplayPopup : ListDisplayPopup
{
    public Dictionary<PetItemStruct, InventoryPetItem> m_petItemDict = new Dictionary<PetItemStruct, InventoryPetItem>();
    //public List<PetItemStruct> m_debugInitPets = new List<PetItemStruct>();

    public List<InventoryPetDisplayButton> m_buttonList = new List<InventoryPetDisplayButton>();

    private InventoryPetDisplayButton m_currentButton;

    public InventoryPetDisplayButton m_buttonPrefab;

    private int m_currentIndex = 0;

    public int m_equipSlotsCount = 3;
    public int m_currentEmptySlot = 0;

    public bool m_currentButtonEmpty = true;

    [Header("Buttons")]
    public ButtonEffectLogic m_equipButton;
    public ButtonEffectLogic m_unequipButton;
    public ButtonEffectLogic m_craftButton;
    public ButtonEffectLogic m_equipBestButton;
    public ButtonEffectLogic m_deleteButton;
    public ButtonEffectLogic m_lockButton;

    public GameObject m_infoZone;

    public TMP_Text m_craftText;

    public PetAssigner m_petAssigner;

    public IPetMultiplier m_petMultiplySetter;

    private List<InventoryPetItem> m_petItemList = new List<InventoryPetItem>();
    public TMP_Text m_multiplierText;
    public TMP_Text m_name;
    public Image m_previewBackground;
    public TMP_Text m_numOfPets;
    public TMP_Text m_equippedAmount;

    public GameObject m_lockObject;
    public GameObject m_unlockObject;

    public Image m_level;
    public Image m_redDot;

    protected Febucci.UI.Core.TAnimCore m_textAnim;
    public void SetRedDot(bool state)
    {
        if (m_redDot.enabled == state) return;
        m_redDot.enabled = state;
    }
    public override void InitButtons()
    {
        m_equipButton.onClick.AddListener(EquipPet);
        m_unequipButton.onClick.AddListener(UnequipPet);
        m_craftButton.onClick.AddListener(CraftCurrentPet);
        m_equipBestButton.onClick.AddListener(AutoSortAndEquipPet);
        m_deleteButton.onClick.AddListener(RemoveCurrentPet);
        m_lockButton.onClick.AddListener(SwitchLockCurrentPet);
    }
    void SwitchLockCurrentPet()
    {
        if (m_currentButtonEmpty) return;
        m_currentButton.SwitchLockState();
    }
    public bool IsFull()
    {

        if (m_currentIndex >= m_buttonList.Count)
        {
            UINotify.Instance.NotifyBag();
            return true;
        }
        return false;
    }

    public override void Show()
    {
        base.Show();
        SetRedDot(false);
    }
    public void AddPetItem(PetItemStruct item, bool saving = true, int amountPlus = 1)
    {
        if (m_petItemDict.ContainsKey(item))
        {
            m_petItemDict[item].m_amount += amountPlus;
        }
        else
        {
            InventoryPetItem itemToAdd = new InventoryPetItem(item.m_petItem, item.m_level, amountPlus);
            m_petItemDict.Add(item, itemToAdd);
            m_petItemList.Add(itemToAdd);
            

        }
        for (int i = 0; i < amountPlus && m_currentIndex < m_buttonList.Count; i++)
        {
            InventoryPetDisplayButton inventoryButton = m_buttonList[m_currentIndex];
            inventoryButton.SetButtonGraphic(m_petItemDict[item]);
            m_currentIndex++;
        }
        m_numOfPets.SetTextFormat("{0}", m_currentIndex);
        SetRedDot(true);

        if (saving)
        {
            ClimbAndJump_DataController.Instance.SetPet(item.m_petItem, item.m_level, m_petItemDict[item].m_amount);
        }
        //AutoSortAndEquipPet();
    }
    void ReorderPets(int st)
    {
        if (m_currentIndex <= st) return;
        m_buttonList.Sort(st, m_currentIndex - st, Comparer<InventoryPetDisplayButton>.Create((a, b) => b.m_petItemData.m_currentMultiplier.CompareTo(a.m_petItemData.m_currentMultiplier)));
        for (int i = 0; i < m_buttonList.Count; i++)
        {
            m_buttonList[i].SetOrder(i);
        }
    }
    public void AutoSortAndEquipPet()
    {
        int st = 0;
        //if (m_currentEmptySlot == -1) st = m_equipSlotsCount;
        //else
        for (int i = 0; i < m_equipSlotsCount; i++) SetEquip(i, false);
        ReorderPets(st);
        //if (m_currentEmptySlot == -1) return;
        for (int i = 0; i < Mathf.Min(m_currentIndex, m_equipSlotsCount); i++)
        {
            SetEquip(i, true);
        }
        m_currentEmptySlot = -1;
        for (int i = 0; i < m_equipSlotsCount; i++)
        {
            if (m_buttonList[i].m_isEquipped) continue;
            m_currentEmptySlot = i;
            break;
        }

    }
    protected virtual void SetEquip(int index, bool isEquipped)
    {
        //this.Log(index);
        //if (index < 0) return;
        if (m_buttonList[index].m_isEquipped == isEquipped)
        {
            this.LogError("same status");
            return;
        }
        m_currentEquipped += (isEquipped ? 1 : -1);
        m_equippedAmount.SetTextFormat("{0}", m_currentEquipped);
        m_buttonList[index].m_isEquipped = isEquipped;

        double multi = m_buttonList[index].m_petItemData.m_currentMultiplier;
        m_petMultiplySetter.ChangeMultiplier((isEquipped ? 1 : -1) * multi);
        m_petAssigner.AssignPet(index, isEquipped ? m_buttonList[index].m_petItemData.m_petItem : null);
        m_buttonList[index].SetEquipGraphic();
    }
    public override void InitList()
    {
        m_textAnim = m_name.GetComponent<Febucci.UI.Core.TAnimCore>();
        for (int i = 0; i < 60; i++)
        {
            InventoryPetDisplayButton button = Instantiate(m_buttonPrefab, m_contentTransform);
            button.InitButton(null);

            m_buttonList.Add(button);
            //button.OnClickAction += DisplayData;
            button.ButtonSelectedAction += SetCurrentButton;
        }
        int[,] pets = ClimbAndJump_DataController.Instance.m_petSave;
        for (int i = 0; i < pets.Length; i++)
        {
            int id = i / 3;
            int level = (i % 3);
            int amount = pets[id, level];
            if (amount <= 0) continue;
            
            AddPetItem(new PetItemStruct(ClimbAndJump_DataController.Instance.m_petData.m_petItems[id], level), false, amount);
        }
        m_currentEmptySlot = 0;
        AutoSortAndEquipPet();

        m_currentButtonEmpty = true;
        m_infoZone.SetActive(false);
        m_equippedAmount.SetTextFormat("{0}", m_currentEquipped);
        m_numOfPets.SetTextFormat("{0}", m_currentIndex);
    }
    protected virtual void SetCurrentButton(InventoryPetDisplayButton button)
    {
        DisplayData(button.m_displayItemData);
       // Debug.LogWarning("button set", button);
        m_currentButton = button;
        m_currentButtonEmpty = (button.m_petItemData == null);
        m_previewBackground.sprite = button.m_bg.sprite;
        if (m_currentButtonEmpty)
        {
            m_infoZone.SetActive(false);
            return;
        }

        if (!m_infoZone.activeSelf) m_infoZone.SetActive(true);
        m_equipButton.gameObject.SetActive(!button.m_isEquipped);
        m_unequipButton.gameObject.SetActive(button.m_isEquipped);

        int amount = button.m_petItemData.m_amount;
        //m_craftButton.gameObject.SetActive(amount >= 3);
        MoneyUIView.UpdateMoney(button.m_petItemData.m_currentMultiplier, m_multiplierText, false);
        //m_multiplierText.SetTextFormat("x{0}",button.m_petItemData.m_currentMultiplier);
        m_craftText.SetTextFormat("Craft to Big({0} / 3)",amount);
        //m_name.SetText(button.m_petItemData.m_petItem.m_itemName);

        m_lockObject.SetActive(!m_currentButton.m_isLocked);
        m_unlockObject.SetActive(m_currentButton.m_isLocked);
        m_level.enabled = m_currentButton.m_levelGraphic.enabled;
        m_level.sprite = m_currentButton.m_levelGraphic.sprite;


    }

    List<InventoryPetDisplayButton> m_cachedRemove = new List<InventoryPetDisplayButton>();
    public void RemovePetItem(PetItemStruct item, int amount = 1)
    {
        m_cachedRemove.Clear();
        if (m_petItemDict.ContainsKey(item))
        {
            m_petItemDict[item].m_amount-= amount;
            if (m_petItemDict[item].m_amount < 0)
            {
                this.LogError("AMOUNT LESS THAN 0");
            }
        }

        for (int i = 0; i < m_buttonList.Count; i++)
        {
            if (m_buttonList[i].m_petItemData == null) continue;
            if (m_buttonList[i].m_petItemData.m_petItem == item.m_petItem && m_buttonList[i].m_petItemData.m_level == item.m_level)
            {
                if (m_buttonList[i].m_isEquipped)
                {
                    SetEquip(i, false); 
                    m_currentEmptySlot = i;
                }

                m_cachedRemove.Add(m_buttonList[i]);
                amount--;
                if (amount == 0) break;
            }
        }
        foreach (var button in m_cachedRemove)
        {
            button.SetButtonGraphic(null);
            m_buttonList.Remove(button);
            m_buttonList.Add(button);
            m_currentIndex--;
            button.transform.SetAsLastSibling();
        }
        m_numOfPets.SetTextFormat("{0}", m_currentIndex);
        ClimbAndJump_DataController.Instance.SetPet(item.m_petItem, item.m_level, m_petItemDict[item].m_amount);
    }
    public override void DisplayData(DisplayItem item)
    {
        if (item == null) return;
        base.DisplayData(item);
        InventoryPetItem inventoryItem = item as InventoryPetItem;
        var petData = inventoryItem.m_petItem;
        if (petData.m_rarityIndex == 4)
        {
            //m_name.SetTextFormat("<rainb>{0}</rainb>", petData.m_itemName);
            m_textAnim.SetText(ZString.Format("<rainb>{0}</rainb>", petData.m_itemName));

        }
        else m_name.SetText(petData.m_itemName);
        //m_amountText.text = inventoryItem.m_amount.ToString();


    }
    void RemoveCurrentPet()
    {
        if (m_currentButtonEmpty) return;
        if (m_currentButton.m_isLocked)
        {
            UINotify.Instance.Notify("Pet is locked! Unlock to Remove");
            return;
        }
        this.Log("Removing....");
        InventoryPetItem currentPetItem = m_currentButton.m_petItemData;
        if (currentPetItem == null)
        {
            this.LogError("Current Pet Item is null");
            return;
        }
        RemovePetItem(new PetItemStruct(currentPetItem.m_petItem, currentPetItem.m_level), 1);
        for (int i = 0; i < m_equipSlotsCount; i++) SetEquip(i, false);
        AutoSortAndEquipPet();
        SetCurrentButton(m_buttonList[0]);
    }
    void CraftCurrentPet()
    {
        if (m_currentButtonEmpty) return;
        this.Log("Crafting....");
        InventoryPetItem currentPetItem = m_currentButton.m_petItemData;
        if (currentPetItem == null)
        {
            this.LogError("Current Pet Item is null");
            return;
        }
        if (currentPetItem.m_amount < 3 || currentPetItem.m_level == 2) return;
        RemovePetItem(new PetItemStruct(currentPetItem.m_petItem, currentPetItem.m_level), 3);
        AddPetItem(new PetItemStruct(currentPetItem.m_petItem, currentPetItem.m_level + 1));
        AutoSortAndEquipPet();
        SetCurrentButton(m_buttonList[m_currentIndex - 1]);
    }

    void EquipPet()
    {
        if (m_currentButtonEmpty) return;
        if (m_currentButton.m_isEquipped) return;
        if (m_currentEmptySlot == -1)
        {
            double minN = double.MaxValue;
            for (int i = 0; i < m_equipSlotsCount; i++)
            {
                if (m_buttonList[i].m_isEquipped && minN > m_buttonList[i].m_petItemData.m_currentMultiplier)
                {
                    minN = m_buttonList[i].m_petItemData.m_currentMultiplier;
                    m_currentEmptySlot = i;
                }
            }
           
        }

        if (m_currentEmptySlot >= 0)
        {
            SetEquip(m_currentEmptySlot, false);
            InventoryPetDisplayButton buttonToBeReplaced = m_buttonList[m_currentEmptySlot];
            if (buttonToBeReplaced != m_currentButton)
            {
                int replaceIndex = m_buttonList.IndexOf(m_currentButton);

                m_buttonList.Remove(buttonToBeReplaced);
                m_buttonList.Remove(m_currentButton);


                m_buttonList.Insert(m_currentEmptySlot, m_currentButton);
                m_buttonList.Insert(replaceIndex, buttonToBeReplaced);
                m_currentButton.transform.SetSiblingIndex(m_currentEmptySlot);
                buttonToBeReplaced.transform.SetSiblingIndex(replaceIndex);
            }
        }
        SetEquip(m_currentEmptySlot, true);
        m_currentEmptySlot = -1;
        for (int i = 0; i < m_equipSlotsCount; i++)
        {
            if (m_buttonList[i].m_isEquipped) continue;
            m_currentEmptySlot = i;
            break;
        }
        
        ReorderPets(m_equipSlotsCount);
        SetCurrentButton(m_currentButton);
    }
    protected int m_currentEquipped = 0;
    void UnequipPet()
    {
        if (m_currentButtonEmpty) return;
        if (!m_currentButton.m_isEquipped) return;

        m_currentEmptySlot = m_buttonList.IndexOf(m_currentButton);
        SetEquip(m_currentEmptySlot, false);

        SetCurrentButton(m_currentButton);
    }

}
