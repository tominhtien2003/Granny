using Cysharp.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWall_DataController : MonoBehaviour
{
    public static PunchWall_DataController Instance;

    private static bool m_specialWallDone;
    public static bool SpecialWallDone
    {
        get => m_specialWallDone;
        set{
            m_specialWallDone = value;
            PlayerPrefs.SetInt("SPWallCutscene", value?1:0);
        }
    }
    public double TrainMultiplier
    {
        get => m_ascendMultiplierData.m_values[m_currentAscension];
    }
    public SO_DoubleData m_ascendRequirementData;
    public SO_DoubleData m_ascendMultiplierData;
    public SO_IntData m_ascendUnlockData;
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < m_bodypartStrength.Length; i++)
        {
            m_bodypartStrength[i] = IdleGameBase_PrefData.GetDouble(ZString.Format("body_part_{0}", i), PrefData.CurrentMinigame, "10");
        }
        m_currentAscension = PlayerPrefs.GetInt("punchwall_current_ascension", 0);
        m_specialWallDone = (PlayerPrefs.GetInt("SPWallCutscene", 0) == 1)?true:false;
    }
    private static int m_currentAscension;
    public static int CurrentAscension
    {
        get { return m_currentAscension; }
        set
        {
            m_currentAscension = value;
            PlayerPrefs.SetInt("punchwall_current_ascension", m_currentAscension);
        }
    }
    public static double[] m_bodypartStrength = new double[3];

    public static void SaveBodypartStrength()
    {
        for (int i = 0; i < m_bodypartStrength.Length; i++)
        {
            IdleGameBase_PrefData.SaveDouble(ZString.Format("body_part_{0}", i), PrefData.CurrentMinigame, m_bodypartStrength[i]);
        }
    }


}
