using EPOOutline;
#if PRIMETWEEN
using PrimeTween;
#elif LIT_MOTION
using LitMotion;
using LitMotion.Extensions;
#elif  DOTWEEN
using DG.Tweening;
#endif
using System.Collections.Generic;
using UnityEngine;

public class MuscleSetter : MonoBehaviour
{
    public MeshRenderer[] m_muscles;
    public SkinnedMeshRenderer[] m_musclesSkinned;
    public List<MeshRenderer> m_armMuscles;
    public List<MeshRenderer> m_bodyMuscles;
    public List<MeshRenderer> m_legMuscles;
    public List<Outlinable> m_outlinables;
    public List<List<MeshRenderer>> m_muscleGroups;
    private Material[] m_mats;
    public Color m_startColor;
    public Color m_endColor;
#if UNITY_EDITOR
    [Button]
    public void InitMuscles()
    {
        m_muscles = GetComponent<PlayerSkinDataSetter>().m_playerMesh.transform.parent.GetComponentsInChildren<MeshRenderer>(true);
        this.SetDirty();
    }
#endif
    private void Awake()
    {
        var v = GetComponent<PlayerSkinDataSetter>();
        v.m_onSkinAssigned += () =>
        {
            foreach (var muscle in m_muscles)
            {
                m_mats = muscle.sharedMaterials;

                m_mats[0] = v.m_playerMesh.sharedMaterials[0];
                muscle.sharedMaterials =m_mats;
            }
            foreach (var muscle in m_musclesSkinned)
            {
                m_mats = muscle.sharedMaterials;

                m_mats[0] = v.m_playerMesh.sharedMaterials[0];
                muscle.sharedMaterials =m_mats;
            }
        };
        m_muscleGroups = new List<List<MeshRenderer>>()
        {
            m_armMuscles,
            m_bodyMuscles,
            m_legMuscles
        };

        /*for (int i= 0; i < 3; i++)
        {
            foreach (var r in m_muscleGroups[i]) m_outlinables[i].AddRenderer(r);
            m_outlinables[i].enabled = false;
        }*/
    }

    public void AnimateMusclesSize(float prevSize, float size, int id)
    {
        foreach (var v in m_muscleGroups[id])
        {
#if PRIMETWEEN
            Tween.Scale(v.transform, prevSize, size, .5f);
#elif LIT_MOTION
            LMotion.Create(Vector3.one * prevSize, Vector3.one * size, .5f).BindToLocalScale(v.transform).AddTo(v.transform);
#elif DOTWEEN
            Code dotween di
#endif
        }
    }
    public void SetMusclesSize(float psize, int id) { 
        foreach (var v in m_muscleGroups[id])
        {
            v.transform.localScale = Vector3.one * psize;
        }
    }
    
#if LIT_MOTION
    private MotionHandle m_handle;
#else
    private Tween _tweenColor;
#endif
    
    public void AnimateMuscleGroup(int id)
    {
        m_outlinables[id].enabled = true;
#if PRIMETWEEN
        if (_tweenColor.active) return;
        _tweenColor = Tween.Custom(m_startColor, m_endColor, .5f, t =>
        {
            m_outlinables[id].FrontParameters.FillPass.SetColor("_PublicColor", t);
        }).OnComplete(() =>
        {
            m_outlinables[id].enabled = false;
        });
#elif LIT_MOTION
        if (m_handle.IsActive()) return;
        m_handle = m_outlinables[id].FrontParameters.FillPass.LColor("_PublicColor", m_startColor, m_endColor, .5f, () =>
        {
            m_outlinables[id].enabled = false;
        }, 2); 
#elif  DOTWEEN
        Code dotween di
#endif
    }
    private void OnDisable()
    {
#if PRIMETWEEN
        if (_tweenColor.IsActive())
        {
            _tweenColor.Kill();
        }
#elif LIT_MOTION
        m_handle.TryCancel();
#elif  DOTWEEN
	    if (_tweenColor != null)
        {
            _tweenColor.Kill();
            _tweenColor = null;
        }
#endif
    }
}
