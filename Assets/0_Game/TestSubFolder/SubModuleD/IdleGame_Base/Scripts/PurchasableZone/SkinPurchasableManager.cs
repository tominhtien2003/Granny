using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISkinControllerUser
{
    public SkinController BloxSkinController { get; set; }
}
public class SkinPurchasableManager : MonoBehaviour, ISkinControllerUser
{
    public SkinPurchasableZone[] m_skinZones;
    public UnlockSkinPopup m_skinPopup;

    public SkinController BloxSkinController { get ; set; }

    private void Awake()
    {
        m_skinZones = GetComponentsInChildren<SkinPurchasableZone>(true);

    }
    private void Start()
    {
        for (int i = 0; i < m_skinZones.Length; i++)
        {
            //m_skinZones[i].InitSkinZone();
            
            m_skinZones[i].OnTryUnlock += OpenPopup;
        }
    }
    public void SetSkinPos(List<Transform> pos)
    {
        for (int i = 0; i < m_skinZones.Length; i++)
        {
            m_skinZones[i].transform.position = pos[i].position;
        }
    }
    public void SetSkinsIds(List<int> ids)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            m_skinZones[i].BloxSkinController = BloxSkinController;
            m_skinZones[i].SetId(ids[i]);
            
            //if (!m_skinZones[i].gameObject.activeSelf) m_skinZones[i].gameObject.SetActive(true);
        }
        for (int i = ids.Count; i < m_skinZones.Length; i++)
        {
            m_skinZones[i].gameObject.SetActive(false);           
        }
    }
    void OpenPopup(SkinPurchasableZone adAmount)
    {
        m_skinPopup.SetSkinData(adAmount);
        m_skinPopup.Show();
    }
}
    