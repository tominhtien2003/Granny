using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Adventure_InventoryUI : MonoBehaviour
{

    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;

    private List<Adventure_InventorySlotUI> slots = new List<Adventure_InventorySlotUI>();

    public void RefreshUI(List<Adventure_ItemInfo> itemInfos)
    {
        foreach (var s in slots)
            Destroy(s.gameObject);
        slots.Clear();

        foreach (var info in itemInfos)
        {
            var go = Instantiate(slotPrefab, slotParent);
            var slot = go.GetComponent<Adventure_InventorySlotUI>();
            slot.Setup(info);
            slots.Add(slot);
        }
    }

    public void UpdateHighlight(Adventure_ItemInfo selectedHandItem, List<Adventure_ItemBase> activeBuffs,Dictionary<Adventure_ItemInfo, Adventure_ItemBase> instances)
    {
        foreach (var slot in slots)
        {
            var info = slot.ItemInfo;

            bool highlight = false;

            if (info.itemType == Adventure_ItemType.HandItem || info.itemType == Adventure_ItemType.Key)
            {
                highlight = (info == selectedHandItem);
            }
            else if (info.itemType == Adventure_ItemType.Buff)
            {
                highlight = activeBuffs.Contains(instances[info]);
            }

            slot.SetHighlight(highlight);
        }
    }

    public void RemoveSlotForItem(Adventure_ItemInfo info)
    {
        var slot = slots.Find(s => s.ItemInfo == info);
        if (slot != null)
        {
            Destroy(slot.gameObject);
            slots.Remove(slot);
        }
    }

}

