using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListDisplayItemButton : MonoBehaviour
{
    public DisplayItem m_displayItemData;
    public ButtonEffectLogic m_selectButton;
    public UnityAction<DisplayItem> OnClickAction;
    public Image m_displayImage;

    public virtual void InitButton(DisplayItem item)
    {
       
        SetButtonGraphic(item);
        m_selectButton.onClick.AddListener(() => OnClickAction?.Invoke(m_displayItemData));
    }

    public virtual void SetButtonGraphic (DisplayItem item)
    {
        m_displayItemData = item;
        if (item != null) m_displayImage.sprite = item.m_displayIcon;
        m_displayImage.enabled = item != null;
    }
}
