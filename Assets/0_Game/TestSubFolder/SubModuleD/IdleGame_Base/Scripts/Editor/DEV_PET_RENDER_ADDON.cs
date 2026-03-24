#if UNITY_EDITOR
using UnityEngine;

public class DEV_PET_RENDER_ADDON : MonoBehaviour
{
    public GameObject m_pet;
    public SO_PetItemDataV2 m_petData;
    public int m_currentIndex;

    public string m_wrapName;
    public string m_folderPath;
    public DEV_RENDER_TEXTURE_EXPORTER m_screenshot;

    private MeshRenderer m_render;
    private MeshFilter m_mesh;
    [Button]
    public void GetRendererAndFilter()
    {
        m_render = m_pet.GetComponent<MeshRenderer>();
        m_mesh = m_pet.GetComponent<MeshFilter>();
    }
    public void IterateList(int dir)
    {
        m_currentIndex += dir;
        if (m_currentIndex < 0) m_currentIndex += m_petData.m_petItems.Length;
        m_currentIndex %= m_petData.m_petItems.Length;
        SetPet(m_currentIndex);
    }
    void SetPet(int index)
    {
        m_render.sharedMaterials = m_petData.m_petItems[index].m_mats;
        m_mesh.sharedMesh = m_petData.m_petItems[index].m_mesh;

        //m_screenshot._fileName = m_folderPath + m_petData.m_petItems[index].m_itemName + ".png";
    }
    [Button]
    public void GoLeft()
    {
        IterateList(-1);
    }
    [Button]
    public void GoRight()
    {
        IterateList(1);
    }
}
#endif