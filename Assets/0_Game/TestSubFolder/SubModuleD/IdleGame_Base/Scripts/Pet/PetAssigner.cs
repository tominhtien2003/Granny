using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAssigner : MonoBehaviour
{
    public List<PetRenderer> m_renderer;
    private void Awake()
    {
        foreach(PetRenderer renderer in m_renderer)
        {
            renderer.gameObject.SetActive(false);
        }
    }
    public void AssignPet(int petIndex, PetItem pet)
    {
        //this.LogError(petIndex);
        if (pet == null)
        {
            //this.Log("pet assigned null " + petIndex);
            m_renderer[petIndex].gameObject.SetActive(false);
            return;
        }
        if (!m_renderer[petIndex].gameObject.activeSelf)
        {
            //this.Log("pet added " + petIndex);
            m_renderer[petIndex].gameObject.SetActive(true);
        }
        m_renderer[petIndex].AssignPet(pet);
    }



}
