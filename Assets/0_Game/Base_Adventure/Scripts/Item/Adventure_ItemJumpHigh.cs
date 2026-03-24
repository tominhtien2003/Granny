using UnityEngine;
using BloxLikeBasic;
public class Adventure_ItemJumpHigh : Adventure_ItemBase
{
    public float highMultiplier;
    float originNormalHigh;
    [SerializeField] NormalMovement normalMovement;
    public override void OnEquip()
    {
        if (normalMovement == null)
        {
            normalMovement = Adventure_PlayerHolder.Instance.GetComponentInChildren<NormalMovement>();
        }
        originNormalHigh = normalMovement.JumpUpSpeed;
        normalMovement.JumpUpSpeed *= highMultiplier;
    }

    public override void OnUnequip()
    {
        normalMovement.JumpUpSpeed = originNormalHigh;
    }

    public override void Use()
    {

    }
}
