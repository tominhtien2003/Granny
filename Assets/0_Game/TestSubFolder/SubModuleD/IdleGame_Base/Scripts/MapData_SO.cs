using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TeleportLocation
{
    public string m_locationName;
    public long m_cupsRequired;
    public GameObject m_locationGameObject;
    public bool m_isUnlocked;
    public float m_climbMultiplier;
    public EggsData m_eggsData;
    public Transform m_cupPos;
    public float m_moneyMultiplier;
    public InteractPosHolder m_posHolder;
    public Sprite m_towerSprite;
    public Sprite m_teleportSprite;
    public string m_name;
    public int m_cupMultiplier;
}
[CreateAssetMenu(fileName = "LocationMapData", menuName = "Data/Location Data")]
public class MapData_SO : ScriptableObject
{
    public List<TeleportLocation> m_locations;
}
