using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_SkinControllerInjector : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        var h = GetComponentsInChildren<ISkinControllerUser>(true);
        foreach (var v in h) v.BloxSkinController = DataController.BloxSkinController;
        //Destroy(this);
    }

}
