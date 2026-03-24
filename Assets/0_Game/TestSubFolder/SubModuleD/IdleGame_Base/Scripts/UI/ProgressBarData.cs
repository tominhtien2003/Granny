using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarData : MonoBehaviour
{
    public List<Image> m_icons = new List<Image>();
    public List<Image> m_raceImages = new List<Image>();
    public Image m_playerRaceImage;
    public Image m_iconPrefab;
    public Transform m_iconParent;
    public Image m_playerIcon;

    public void Init(int length)
    {
        Vector3 pos = m_playerIcon.rectTransform.anchoredPosition;
        for (int i = 0; i < length; i++)
        {
            Image icon = Instantiate(m_iconPrefab, m_iconParent);
            icon.rectTransform.anchoredPosition = pos;
            m_icons.Add(icon);
            m_raceImages.Add(icon.transform.GetChild(0).GetComponent<Image>());
        }
        m_playerIcon.transform.SetAsLastSibling();
        m_playerRaceImage = m_playerIcon.transform.GetChild(0).GetComponent<Image>();
    }
}
