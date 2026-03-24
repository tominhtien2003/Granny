using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_BigFan : MonoBehaviour, IActiveBySwitch
{
    [SerializeField] List<GameObject> fanColliders = new List<GameObject>();

    public void Active()
    {
        foreach (var go in fanColliders)
        {
            go.SetActive(true);
        }
    }
}
