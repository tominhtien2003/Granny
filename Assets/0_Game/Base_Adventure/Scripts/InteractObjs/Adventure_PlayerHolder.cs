using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_PlayerHolder : MonoBehaviour
{
    public static Adventure_PlayerHolder Instance;
    [SerializeField] protected Transform handPoint;

    void Awake() => Instance = this;

    public Transform HandPoint => handPoint;

    public virtual void ShowItem(Adventure_ItemBase item)
    {
        item.handParent = handPoint;
        item.OnEquip();
    }

    public virtual void HideItem(Adventure_ItemBase item)
    {
        item.OnUnequip();
    }

}
