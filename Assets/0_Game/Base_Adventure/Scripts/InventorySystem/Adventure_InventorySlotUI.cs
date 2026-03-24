using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Adventure_InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image highlight;
    [SerializeField] private ButtonEffectLogic btn;
    //public G3_ItemBase Item { get; private set; }
    private Adventure_ItemInfo itemInfo;
    public Adventure_ItemInfo ItemInfo => itemInfo;

    private void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }
    public void Setup(Adventure_ItemInfo info)
    {
        itemInfo = info;
        icon.sprite = info.icon;
        itemNameText.text = info.itemName;
    }

    public void SetHighlight(bool on)
    {
        highlight.enabled = on;
    }

    public void OnClick()
    {
        Adventure_InventoryManager.Instance.SelectItem(ItemInfo);
    }

}
