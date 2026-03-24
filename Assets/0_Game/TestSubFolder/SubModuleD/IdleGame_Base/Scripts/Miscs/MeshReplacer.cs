using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
public class MeshReplacer : MonoBehaviour
{
    public Mesh newMesh;
    [Button]
    public void ReplaceMesh()
    {
        MeshFilter[] m = GetComponentsInChildren<MeshFilter>();
        foreach (var i in m)
        {
            i.sharedMesh = newMesh;
            i.SetDirty();
        }

    }
}
#endif