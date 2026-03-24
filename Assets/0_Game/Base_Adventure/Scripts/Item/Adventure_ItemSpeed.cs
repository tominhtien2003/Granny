using UnityEngine;
using BloxLikeBasic;
public class Adventure_ItemSpeed : Adventure_ItemBase
{
    public float speedMultiplier;
    float originNormalSpeed;
    [SerializeField] NormalMovement normalMovement;
    public override void OnEquip()
    {
        if(normalMovement == null)
        {
            normalMovement = Adventure_PlayerHolder.Instance.GetComponentInChildren<NormalMovement>();
        }
        originNormalSpeed = normalMovement.MaxStableMoveSpeed;
        normalMovement.MaxStableMoveSpeed *= speedMultiplier;
    }

    public override void OnUnequip()
    {
        normalMovement.MaxStableMoveSpeed = originNormalSpeed;
    }

    public override void Use()
    {
        
    }
}
