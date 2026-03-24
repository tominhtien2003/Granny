using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPetRotation : MonoBehaviour
{
    public List<PetRenderer> m_ren;

    public List<Transform> m_idlePos;
    public List<Transform> m_rotPos;

    public void SetRendererStates(bool isRotate)
    {
        List<Transform> vec = isRotate? m_rotPos : m_idlePos;
        for (int i = 0; i < m_ren.Count; i++)
        {
            m_ren[i].transform.localPosition = transform.InverseTransformPoint(vec[i].transform.position);
            m_ren[i].transform.eulerAngles = vec[i].transform.eulerAngles;
        }
    }
    public void SetPetSkins(List<PetItem> m_pets)
    {
        for (int i = 0; i < m_ren.Count; i++ )
        {
            m_ren[i].AssignPet(m_pets[i]);
        }
    }

}
