using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTag : MonoBehaviour
{
    public GameObject[] obj;
    [Button]
    void Change()
    {
        obj = GameObject.FindGameObjectsWithTag("KillBlock");
        foreach (GameObject go in obj)
        {
            go.tag = "Kill";
        }
    }
}
