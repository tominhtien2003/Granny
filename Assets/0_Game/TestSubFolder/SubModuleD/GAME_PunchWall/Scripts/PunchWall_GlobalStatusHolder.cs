using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PunchWall_GlobalStatusHolder : MonoBehaviour
{
    public static int CurrentWall;
    public static float MaxAutoAimDistance = 6;
    public static int CurrentBodypartId;
    public float WallDistance;
    public float WallStartOffset;
    public static PunchWall_GlobalStatusHolder Instance;
    public List<double> WallHps = new List<double>();
    public List<double> TrueWallHps = new List<double>();
    public List<double> CurrentWallHps;
    public int m_wallAmount;
    public static bool Punching = false;
    public static double WallMultiplier = 1;
    public WallStats m_wall;
    public Image m_ascendButtonDot;
    public List<SpecialWall> m_specialWalls;

    public int m_activeSpecialWalls;
    public static Action OnTrain;
    public static bool CanReturn;
    public void ResetWall()
    {
        m_wall.transform.localPosition = new Vector3(WallStartOffset, 0, 0);
        m_wall.SetId(0);
        m_wall.SetHp(WallHps[0] * WallMultiplier, WallHps[0] * WallMultiplier);
        for (int i = 0; i < WallHps.Count; i++)
        {
            TrueWallHps[i] = WallHps[i] * WallMultiplier;
            CurrentWallHps[i] = TrueWallHps[i];
        }

    }
    public static int m_cnt = 0;
    public static bool[] m_passedCheck = { false, false, false };
    public static Action<int, int> OnBodypartReachedRequirement;
    public void CheckBodypartRequirement(int id)
    {
        if (m_passedCheck[id]) return;
        m_passedCheck[id] = (PunchWall_DataController.m_bodypartStrength[id] >= PunchWall_DataController.Instance.m_ascendRequirementData.m_values[PunchWall_DataController.CurrentAscension]);
        if (m_passedCheck[id])
        {
            OnBodypartReachedRequirement?.Invoke(id, PunchWall_DataController.CurrentAscension);
            m_cnt++;
        }
        m_ascendButtonDot.enabled = (m_cnt >= 3);
    }
    public void ResetRequirement()
    {
        m_cnt = 0;
        for (int i = 0; i < 3; i++)
        {
            m_passedCheck[i] = (PunchWall_DataController.m_bodypartStrength[i] >= PunchWall_DataController.Instance.m_ascendRequirementData.m_values[PunchWall_DataController.CurrentAscension]);
            if (m_passedCheck[i]) m_cnt++;
        }
        m_ascendButtonDot.enabled = (m_cnt >= 3);
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //m_wall = FindObjectOfType<WallStats>(); 
        CurrentWallHps = new List<double>(WallHps);
        TrueWallHps = new List<double>(WallHps);
        
    }
    private void Start()
    {
        ResetRequirement();
    }
#if UNITY_EDITOR
    [Button]
    public void InitWalls()
    {
        long st = 30;
        WallHps.Clear();
        for (int i = 0; i < m_wallAmount; i++)
        {
            WallHps.Add(st);
            st = (long)(st * Random.Range(1.4f, 1.6f));
        }
        this.SetDirty();
    }
#endif
}
