using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeleportItemButtonAddon : MonoBehaviour
{
    public ButtonEffectLogic m_teleportButton;
    public TMP_Text m_cupText;

    public GameObject m_buy;
    public GameObject m_teleport;
    public TeleportLocation m_locationData;
    public UnityAction<TeleportItemButtonAddon> OnClick;
    public Image m_image;
    public TMP_Text m_locationName;

    public void Init(TeleportLocation location)
    {
        m_locationData = location;
        m_cupText.SetTextFormat("x{0}", m_locationData.m_cupsRequired);
        m_teleportButton.onClick.AddListener(()=>OnClick(this));
        m_image.sprite = location.m_teleportSprite;
        m_locationName.SetText(location.m_name);
        SetUnlockedState();
    }

    public void SetUnlockedState()
    {
        m_cupText.enabled = !m_locationData.m_isUnlocked;
        m_buy.SetActive(!m_locationData.m_isUnlocked);
        m_teleport.SetActive(m_locationData.m_isUnlocked);
    }

}
