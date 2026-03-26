using UnityEngine;

[CreateAssetMenu(menuName = "Anchors/TransformAnchor")]
public class SOTransformAnchor : ScriptableObject
{
    public Transform Anchor;
    public void Set(Transform _anchor)
    {
        Anchor = _anchor;
    }
    public void UnSet()
    {
        Anchor = null;
    }
}
