using System.Collections.Generic;
using UnityEngine;

public class Adventure_InventoryManager : MonoBehaviour
{
    public static Adventure_InventoryManager Instance;

    public Adventure_ItemDatabase itemDatabase;
    public Adventure_InventoryUI ui;

    private List<Adventure_ItemInfo> items = new List<Adventure_ItemInfo>();

    private Dictionary<Adventure_ItemInfo, Adventure_ItemBase> instances = new Dictionary<Adventure_ItemInfo, Adventure_ItemBase>();

    private Adventure_ItemInfo selectedHandItem;
    private Adventure_ItemBase selectedHandInstance;


    private List<Adventure_ItemBase> activeBuffs = new List<Adventure_ItemBase>();
    private void Awake() => Instance = this;

    public void AddItem(string itemName)
    {
        var info = itemDatabase.GetItem(itemName);
        if (info == null) return;
        if (items.Contains(info)) return;

        items.Add(info);

        if (!instances.ContainsKey(info))
        {
            var inst = Instantiate(info.itemPrefab);
            inst.gameObject.SetActive(false);
            instances.Add(info, inst);
        }

        ui.RefreshUI(items);
        SelectItem(info);
    }


    public void SelectItem(Adventure_ItemInfo info)
    {
        if (info.itemType == Adventure_ItemType.HandItem || info.itemType == Adventure_ItemType.Key)
        {
            ToggleHandItem(info);
        }
        else if (info.itemType == Adventure_ItemType.Buff)
        {
            ToggleBuff(info);
        }
    }

    void ToggleHandItem(Adventure_ItemInfo info)
    {
        if (selectedHandItem == info)
        {
            DeselectItem();
            return;
        }

        if (selectedHandInstance != null)
            selectedHandInstance.OnUnequip();

        selectedHandItem = info;
        selectedHandInstance = instances[info];

        Adventure_PlayerHolder.Instance.ShowItem(selectedHandInstance);
        ui.UpdateHighlight(selectedHandItem, activeBuffs, instances);

    }
    void ToggleBuff(Adventure_ItemInfo info)
    {
        var inst = instances[info];

        if (activeBuffs.Contains(inst))
        {
            inst.OnUnequip();
            inst.gameObject.SetActive(false);
            activeBuffs.Remove(inst);
        }
        else
        {
            inst.gameObject.SetActive(true);
            inst.OnEquip();
            activeBuffs.Add(inst);
        }

        ui.UpdateHighlight(selectedHandItem, activeBuffs, instances);

    }

    public void ActivateBuff(Adventure_ItemInfo info)
    {
        var inst = instances[info];

        if (activeBuffs.Contains(inst)) return;

        inst.gameObject.SetActive(true);
        inst.OnEquip(); 
        activeBuffs.Add(inst);
    }

    public void DeselectItem()
    {
        if (selectedHandInstance != null)
        {
            selectedHandInstance.OnUnequip();
        }

        selectedHandInstance = null;
        selectedHandItem = null;

        ui.UpdateHighlight(selectedHandItem, activeBuffs, instances);

    }


    public void RemoveItem(Adventure_ItemInfo info)
    {
        if (!items.Contains(info)) return;
        if ((info.itemType == Adventure_ItemType.HandItem || info.itemType == Adventure_ItemType.Key) &&
            info == selectedHandItem)
        {
            DeselectItem();
        }

        if (info.itemType == Adventure_ItemType.Buff)
        {
            var inst = instances[info];
            if (activeBuffs.Contains(inst))
            {
                inst.OnUnequip();
                activeBuffs.Remove(inst);
            }
        }

        items.Remove(info);
        ui.RefreshUI(items);
    }


    public void RemoveItemFromInstance(Adventure_ItemBase inst)
    {
        foreach (var kv in instances)
        {
            if (kv.Value == inst)
            {
                RemoveItem(kv.Key);
                return;
            }
        }
    }
    public void OnPlayerDied()
    {
        DeselectItem();

        foreach (var buff in activeBuffs)
        {
            buff.OnUnequip();
            buff.gameObject.SetActive(false);
        }
        activeBuffs.Clear();

        items.RemoveAll(i => i.itemType != Adventure_ItemType.Key);
        ui.RefreshUI(items);
    }


}
