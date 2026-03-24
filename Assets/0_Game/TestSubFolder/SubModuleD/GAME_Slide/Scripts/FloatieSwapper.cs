using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatieSwapper : MonoBehaviour
{
    public Transform m_standingPos;
    public Transform m_sittingPos;
    public Transform m_floatie;
    public WingsContainer m_container;

    private List<WingDisplayData> m_wingsList;
    private bool m_cachedState;
    private void Awake()
    {
        m_container = GetComponent<WingsContainer>();
    }
    private void Start()
    {
        m_wingsList = ClimbAndJump_DataController.Instance.m_wingShopData.m_wingShopItemData;
    }

    public void SetFloatiePos(bool state)
    {
        state = !state;
        m_floatie.parent = state ? m_standingPos : m_sittingPos;
        m_floatie.localPosition = state? Vector3.zero: m_wingsList[m_container.m_id].m_wingsData.m_overridePos;
        m_floatie.localEulerAngles = state? Vector3.zero: m_wingsList[m_container.m_id].m_wingsData.m_overrideRot;
        m_floatie.localScale = state? Vector3.one: m_wingsList[m_container.m_id].m_wingsData.m_overrideScale;
    }
}
