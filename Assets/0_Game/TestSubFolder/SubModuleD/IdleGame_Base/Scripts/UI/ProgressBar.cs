using Cysharp.Text;
using KinematicCharacterController;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public SimpleAIScript[] m_bots;

    public KinematicCharacterMotor m_player;
    private List<Image> m_icons;
    private Image m_playerIcon;
    public TMP_Text m_playerHeight;

    public Transform m_iconParent;
    public BoxCollider m_truss;

    public float m_trussHeight;
    private float m_imageHeight;

    public int m_fakeHeight = 12000;

    public RectTransform[] m_heightCPs;
    public int m_xPos = 127;
    public float[] heights = { 2, 5, 8, 11 };
#if UNITY_EDITOR
    [Button]
    public void SetHeightCheckpoints()
    {
        m_imageHeight = m_iconParent.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < heights.Length; i++)
        {
            m_heightCPs[i].anchoredPosition = new Vector2(0, (heights[i] * 1000f) / m_fakeHeight * m_imageHeight);
        }
        m_trussHeight = m_truss.bounds.extents.y + m_truss.bounds.center.y;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    private void Awake()
    {
        ProgressBarData m_data = GetComponent<ProgressBarData>();
        
        m_bots = FindObjectsOfType<SimpleAIScript>(true);
        
        m_data.Init(m_bots.Length);
        m_playerIcon = m_data.m_playerIcon;
        m_icons = m_data.m_icons;
        m_playerIcon.transform.SetAsLastSibling();

        m_imageHeight = m_iconParent.GetComponent<RectTransform>().rect.height;

        
        m_player = FindObjectOfType<KinematicCharacterMotor>();
     

    }

    private int m_cachedHeight;
    private void Update()
    {
        for (int i = 0; i < m_bots.Length; i++)
        {
            float progress = m_bots[i].transform.position.y / m_trussHeight;
            
            m_icons[i].rectTransform.anchoredPosition = new Vector2(m_xPos, progress * m_imageHeight);
        }

        float progressa = m_player.transform.position.y / m_trussHeight;
        m_playerIcon.rectTransform.anchoredPosition = new Vector2(m_xPos, progressa * m_imageHeight);
        //Debug.Log(progressa * 12000);
        int height = Mathf.FloorToInt(progressa * m_fakeHeight);
        //Debug.Log(height);
        height = Mathf.Max(height, 0);
        if (m_cachedHeight == height) return; 
        //Debug.Log(height);
        m_playerHeight.SetTextFormat("{0}m", height);
        m_cachedHeight = height;
    }
}
