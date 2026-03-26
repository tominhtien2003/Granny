using UnityEngine;

public class SetTransformAnchor : MonoBehaviour
{
    [SerializeField] private SOTransformAnchor anchor;
    private void OnEnable()
    {
        anchor.Set(transform);
    }
    private void OnDisable()
    {
        anchor.UnSet();
    }
}
