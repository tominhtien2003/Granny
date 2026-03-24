using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CAJ_SkinControllerInjector : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        var h = GetComponentsInChildren<ISkinControllerUser>(true);
        foreach (var v in h) v.BloxSkinController = ClimbAndJump_DataController.BloxSkinController;
        Destroy(this);
    }
}
