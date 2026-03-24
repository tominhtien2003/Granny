using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPurchasableManager : MonoBehaviour
{
    public WingPurchasableZone[] m_wingZones;
    public UnlockWingPopup m_wingPopup;
    private void Awake()
    {
        m_wingZones = GetComponentsInChildren<WingPurchasableZone>(true);

    }
    private void Start()
    {
        for (int i = 0; i < m_wingZones.Length; i++)
        {
            m_wingZones[i].InitWingZone();
            m_wingZones[i].OnTryUnlock += OpenPopup;
        }
    }
    public void SetWingPos(List<Transform> pos)
    {
        for (int i = 0; i < m_wingZones.Length; i++)
        {
            m_wingZones[i].transform.position = pos[i].position;
        }
    }
    public void SetWingsIds(List<int> ids)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            m_wingZones[i].SetId(ids[i]);
            if (!m_wingZones[i].gameObject.activeSelf) m_wingZones[i].gameObject.SetActive(true);
        }
        for (int i = ids.Count; i < m_wingZones.Length; i++)
        {
            m_wingZones[i].gameObject.SetActive(false);
        }
    }
    void OpenPopup(IAdAmount adAmount)
    {
        m_wingPopup.SetWingData(adAmount);
        m_wingPopup.Show();
    }
    
}
