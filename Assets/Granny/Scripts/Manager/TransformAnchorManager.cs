using UnityEngine;

public class TransformAnchorManager : MonoBehaviour
{
    public static TransformAnchorManager Instance;
    [SerializeField] private SOTransformAnchor playerAnchor;

    private void Awake()
    {
        Instance = this;
    }
    public Transform GetTransformPlayer()
    {
        return playerAnchor.Anchor;
    }
}
