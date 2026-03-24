using System;
using UnityEngine;

public class WingsContainer : MonoBehaviour
{
    public ISpeedChanger m_stats;

    public MeshRenderer[] m_wingsRenderers;
    public MeshFilter[] m_filters;
   
    public int m_id;
    public Action OnWingAssigned;
}
