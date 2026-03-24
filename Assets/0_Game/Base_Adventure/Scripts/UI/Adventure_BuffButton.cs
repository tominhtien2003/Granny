using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Adventure_BuffButton : MonoBehaviour
{
    public ButtonEffectLogic button;
    public Image icon;
    public TextMeshProUGUI describeText;
    public Adventure_ItemDatabase itemDatabase;
    public float changeInterval = 8f;

    Adventure_ItemInfo currentItem;
    float timer;

    void Start()
    {
        RollNewItem();
        button.onClick.AddListener(UseBuff);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
            RollNewItem();
    }

    void RollNewItem()
    {
        timer = 0f;
        List<Adventure_ItemInfo> rollableItems = itemDatabase.items
        .FindAll(item => item.canRoll);

        if (rollableItems.Count == 0) return;
        currentItem = rollableItems[
            Random.Range(0, rollableItems.Count)
        ];

        icon.sprite = currentItem.icon;
        describeText.text = currentItem.describe;
    }

    void UseBuff()
    {
        if (currentItem == null) return;

        Adventure_InventoryManager.Instance.AddItem(currentItem.itemName);

        RollNewItem(); 
    }
}
