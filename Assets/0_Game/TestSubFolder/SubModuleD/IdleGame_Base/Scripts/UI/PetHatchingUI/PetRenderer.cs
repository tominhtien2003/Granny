using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetRenderer : MonoBehaviour
{
    public MeshRenderer m_renderer;
    public MeshFilter m_filter;

    public TrailRenderer m_trail;
    public GameObject m_particle;
    public void AssignPet(PetItem pet)
    {
        m_renderer.sharedMaterials = pet.m_mats;
        m_filter.sharedMesh = pet.m_mesh;
        m_trail.material = pet.m_trailMat;

        if (m_particle != null) m_particle.SetActive(pet.m_rarityIndex >= 3);
    }
   
}
