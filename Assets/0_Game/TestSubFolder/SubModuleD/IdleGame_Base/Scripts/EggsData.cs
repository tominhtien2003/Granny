using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EggInfo
{
    public long m_price;
    public Vector3 m_position;
}
[CreateAssetMenu(fileName = "Eggs_Data", menuName = "Data/Eggs_Data")]
public class EggsData : ScriptableObject
{
    public int m_currentEggStart;
    public List<EggInfo> m_eggs = new List<EggInfo>();
}
