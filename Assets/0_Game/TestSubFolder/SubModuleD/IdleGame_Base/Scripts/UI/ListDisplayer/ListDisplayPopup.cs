using UnityEngine;
using UnityEngine.UI;

public abstract class ListDisplayPopup : BasePopup, IInitable
{
    public Image m_previewImage;
    public Transform m_contentTransform;

    public abstract void InitList();
    public abstract void InitButtons();
    public virtual void DisplayData(DisplayItem item)
    {
        if (item == null)
        {
            m_previewImage.enabled = false;
            return;
        }
        if (!m_previewImage.enabled) m_previewImage.enabled = true;
        m_previewImage.sprite = item.m_displayIcon;
    }

    public void Init()
    {
        InitButtons();
        InitList();
    }
}
