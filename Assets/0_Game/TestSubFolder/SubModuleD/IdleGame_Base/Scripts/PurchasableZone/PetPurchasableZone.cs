using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PetPurchasableZone : BaseAdZone
{
    public int m_petIndex;
    public override int ItemId { get => m_petIndex; set => m_petIndex = value; }
    public UnityAction<PetPurchasableZone> OnTryUnlock;

    public MeshRenderer m_renderer;
    public MeshFilter m_meshFilter;

    public int m_currentAdAmount;
    public int m_totalAdAmount;

    public GameObject m_opText;
    public TextAnimator_TMP m_opAnimator;

    public void InitPetZone()
    {
        PetItem pet = ClimbAndJump_DataController.Instance.m_petData.m_petItems[m_petIndex];
        m_renderer.sharedMaterials = pet.m_mats;
        m_meshFilter.sharedMesh = pet.m_mesh;
        m_totalAdAmount = pet.m_itemAdAmount;
        SetRing(Mathf.Clamp(pet.m_rarityIndex - 2, 0, 2));
        bool isOp = m_petIndex >= 26;
        m_opText.SetActive(isOp);
        m_opAnimator.enabled = isOp;
    }
    public void SetId(int id)
    {
        m_petIndex = id;
        InitPetZone();
    }
    protected override void TryUnlock()
    {
        OnTryUnlock?.Invoke(this);
    }
}
