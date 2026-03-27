using UnityEngine;

public class SetTransformAnchor : MonoBehaviour
{
    [SerializeField] private SOTransformAnchor anchor;
    private void Awake()
    {
        anchor.Set(transform);
        //Debug.Log(anchor.Anchor);
    }
    private void OnDisable()
    {
        anchor.UnSet();
    }
}
