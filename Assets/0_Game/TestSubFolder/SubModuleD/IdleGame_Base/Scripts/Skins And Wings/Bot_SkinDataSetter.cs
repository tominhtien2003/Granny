#if PRIMETWEEN
using PrimeTween;
#elif LIT_MOTION
using LitMotion;
using LitMotion.Extensions;
#elif  DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using KinematicCharacterController;
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot_SkinDataSetter : MonoBehaviour,ISkinControllerUser
{
    public ProgressBarData m_progressBar;
    public SkinController BloxSkinController { get; set; }
    public SkinnedMeshRenderer[] m_ais;
    public WingsContainer[] m_aisWingContainer;

#if UNITY_EDITOR
    [Button]
    void FindAllSkins()
    {
        List<SkinnedMeshRenderer> sks = FindObjectsOfType<SkinnedMeshRenderer>(true).ToList();
        for (int i = 0; i < sks.Count; i++)
        {
            var p = sks[i].GetComponentInParent<KinematicCharacterMotor>();
            if (p == null || !p.gameObject.CompareTag("AI"))
            {
                sks.Remove(sks[i]);
                i--;
            }
            else Debug.Log("h", sks[i]);
        }
        m_ais = sks.ToArray();
        for(int i= 0; i < m_ais.Length; i++)
        {
            m_aisWingContainer[i] = m_ais[i].transform.parent.GetComponentInChildren<WingsContainer>(true);
        }
        EditorUtility.SetDirty(this);
    }
#endif
    void Start()
    {
        InitBotSkins();
    }
    public void InitBotSkins()
    {
        List<SkinShopData> skins = BloxSkinController.m_skinData.m_shopItemData;
        if (m_progressBar != null)
        {
            for (int i = 0; i < m_ais.Length; i++)
            {
                SkinShopData shopData = skins[Random.Range(0, skins.Count)];
                SkinAssignUtils.AssignModel(m_ais[i], shopData);
                StartCoroutine(IEWaitRace(i, shopData.m_raceIcon));
            }
        }
        else
        {
            for (int i = 0; i < m_ais.Length; i++)
            {
                SkinShopData shopData = skins[Random.Range(0, skins.Count)];
                SkinAssignUtils.AssignModel(m_ais[i], shopData);
            }
        }
        List<WingDisplayData> wings = BloxSkinController.m_wingData.m_wingShopItemData;
        for (int i = 0; i < m_aisWingContainer.Length; i++)
        {
            WingDisplayData wingData = wings[Random.Range(0, wings.Count)];
            AssignPlayerModel(m_aisWingContainer[i], wingData);
        }
    }

    private IEnumerator IEWaitRace(int index, Sprite raceIcon)
    {
        yield return null;
        yield return null;
        if (m_progressBar.m_raceImages.Count > 0) 
        {
            m_progressBar.m_raceImages[index].sprite = raceIcon;
        }
    }
    
    public virtual void AssignPlayerModel(WingsContainer wingsContainer, WingDisplayData shopData)
    {
        if (shopData == null || BloxSkinController.m_skinData.m_shopItemData[BloxSkinController.UserCurrentSkin].m_wingAvailable)
        {
            foreach (var w in wingsContainer.m_wingsRenderers) w.enabled = false;
            return;
        }
        foreach (var wing in wingsContainer.m_filters)
        {
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_mesh);
        }
        foreach (var wing in wingsContainer.m_wingsRenderers)
        {
            wing.enabled = true;
            SkinAssignUtils.AssignModel(wing, shopData.m_wingsData.m_materials);
        }
    }
}
