using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchWallProgressBar : MonoBehaviour
{
    ProgressBarData m_barData;
    KinematicCharacterMotor m_player;
    public SimpleAIScript[] m_ais;
    public Transform m_firstGate;
    public Transform m_lastGate;
    private List<Image> m_icons;
    private Image m_playerIcon;
    float m_diff;
    float m_imageHeight;
    void Awake()
    {
        m_barData = GetComponent<ProgressBarData>();
        m_player = FindObjectOfType<KinematicCharacterMotor>();
        m_ais = FindObjectsOfType<SimpleAIScript>(true);
        m_barData.Init(m_ais.Length);
        m_diff = Mathf.Abs(m_lastGate.position.x - m_firstGate.position.x);
        m_playerIcon = m_barData.m_playerIcon;
        m_icons = m_barData.m_icons;
        m_playerIcon.transform.SetAsLastSibling();

        m_imageHeight = m_barData.m_iconParent.GetComponent<RectTransform>().rect.height;

    }

    void Update()
    {
        for (int i = 0; i < m_ais.Length; i++)
        {
            float dist = Mathf.Max(0, -m_ais[i].transform.position.x + m_firstGate.position.x);
            float perc = dist / m_diff;
            m_icons[i].rectTransform.anchoredPosition = new Vector2(52, perc * m_imageHeight);
        }
        float dist2 = Mathf.Max(0,-m_player.transform.position.x + m_firstGate.position.x);
        float perc2 = dist2 / m_diff;
        Vector3 pos2 = m_playerIcon.rectTransform.anchoredPosition;
        m_playerIcon.rectTransform.anchoredPosition = new Vector2(52, perc2 * m_imageHeight);
    }
}
