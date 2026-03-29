using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data",menuName = "Item Data/Item")]
public class SOItemData : ScriptableObject
{
    public string Name;
    [TextArea(2, 4)]
    public string Description;
    public ItemType Type;
    public GameObject Prefab;
    public AudioClip DropSound;
}
