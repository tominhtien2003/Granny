using System.Collections.Generic;
using UnityEngine;

public class InteractPosHolder : MonoBehaviour
{
    public List<Transform> m_eggPos;
    public List<Transform> m_petPos;
    public List<Transform> m_wingPos;
    public List<Transform> m_skinPos;
    public Transform m_portalPos;


    public List<Transform> m_shopPos;
    public List<int> m_petIds;
    public List<int> m_wingsIds;
    public List<int> m_skinIds;
#if UNITY_EDITOR
    public PetPurchasableManager m_petManager;
    public EggsManager m_eggManager;
    public WingPurchasableManager m_wingsManager;
    public SkinPurchasableManager m_skinsManager;


    [Button]
    public void InitPoses()
    {
        PetPurchasableZone[] zones = m_petManager.GetComponentsInChildren<PetPurchasableZone>(true);
        for (int i = 0; i < zones.Length; i++) m_petPos[i].position = zones[i].transform.position;

        EggPurchasableZone[] zones2 = m_eggManager.GetComponentsInChildren<EggPurchasableZone>(true);
        for (int i = 0; i < zones2.Length; i++) m_eggPos[i].position = zones2[i].transform.position;

        WingPurchasableZone[] zones3 = m_wingsManager.GetComponentsInChildren<WingPurchasableZone>(true);
        for (int i = 0; i < zones3.Length; i++) m_wingPos[i].position = zones3[i].transform.position;

        SkinPurchasableZone[] zones4 = m_skinsManager.GetComponentsInChildren<SkinPurchasableZone>(true);
        for (int i = 0; i < zones4.Length; i++) m_skinPos[i].position = zones4[i].transform.position;

        this.SetDirty();
    }
#endif
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform t in m_eggPos)
        {
            if (t != null) Gizmos.DrawRay(t.position, Vector3.up * 100f);
        }
        Gizmos.color = Color.blue;
        foreach (Transform t in m_petPos)
        {
            if (t != null) Gizmos.DrawRay(t.position, Vector3.up * 100f);
        }
        Gizmos.color = Color.magenta;
        foreach (Transform t in m_wingPos)
        {
            if (t != null) Gizmos.DrawRay(t.position, Vector3.up * 100f);
        }
        Gizmos.color = Color.black;
        foreach (Transform t in m_skinPos)
        {
            if (t != null) Gizmos.DrawRay(t.position, Vector3.up * 100f);
        }
    }
}
