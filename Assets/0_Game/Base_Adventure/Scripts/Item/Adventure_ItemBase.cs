using UnityEngine;

public abstract class Adventure_ItemBase : MonoBehaviour
{
    public Transform handParent;
    [SerializeField] Vector3 localPos;
    [SerializeField] Vector3 localRot;
    public bool isHolderItem;
    public virtual void OnEquip()
    {
        SetActiveObj(true);
        transform.SetParent(handParent, false);
        transform.localPosition = localPos;
        transform.localRotation = Quaternion.Euler(localRot);
    }

    public virtual void OnUnequip()
    {
        SetActiveObj(false);
    }
    public void SetActiveObj(bool active)
    {
        if (!isHolderItem) return;
        gameObject.SetActive(active);
    }
    public abstract void Use();
}
