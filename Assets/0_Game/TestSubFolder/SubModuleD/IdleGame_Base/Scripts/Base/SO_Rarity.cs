
using UnityEngine;
[System.Serializable]
public class RarityColor
{
    public string m_rarity;
    public Color m_color;
    public Sprite m_buttonSprite;
}
[CreateAssetMenu(fileName = "Rarity_Data", menuName = "Data/Rarity_Data")]
public class SO_Rarity : ScriptableObject
{
    public RarityColor[] m_rarityColors;
}
