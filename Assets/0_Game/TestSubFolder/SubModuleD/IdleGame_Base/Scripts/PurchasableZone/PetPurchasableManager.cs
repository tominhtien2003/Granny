using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPurchasableManager : MonoBehaviour
{
    public PetPurchasableZone[] m_petZones;
    public UnlockPetPopup m_petPopup;
    private void Awake()
    {
        m_petZones = GetComponentsInChildren<PetPurchasableZone>(true);

    }
    private void Start()
    {
        for (int i = 0; i < m_petZones.Length; i++)
        {
            m_petZones[i].InitPetZone();
            m_petZones[i].OnTryUnlock += OpenPopup;
        }
    }
    public void SetPetPos(List<Transform> pos)
    {
        for (int i = 0; i < m_petZones.Length; i++)
        {
            m_petZones[i].transform.position = pos[i].position;
        }
    }
    public void SetPetIds(List<int> ids)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            m_petZones[i].SetId(ids[i]);
            if (!m_petZones[i].gameObject.activeSelf) m_petZones[i].gameObject.SetActive(true);
        }
        for (int i = ids.Count; i < m_petZones.Length; i++)
        {
            m_petZones[i].gameObject.SetActive(false);
        }
    }
    void OpenPopup(PetPurchasableZone petzone)
    {
        m_petPopup.SetPetData(petzone);
        m_petPopup.Show();
    }
}

