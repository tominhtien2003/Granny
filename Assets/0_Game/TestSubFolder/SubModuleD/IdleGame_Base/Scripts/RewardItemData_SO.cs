using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RewardInfo
{
    public RewardInfoType_SO m_infoType;
    public long m_amount;
    public int m_petId;
    public int m_chance;
}
[CreateAssetMenu(fileName = "Rewards_Data", menuName = "Data/Rewards_Data")]
public class RewardItemData_SO : ScriptableObject
{
    public List<RewardInfo> m_rewards;
}
