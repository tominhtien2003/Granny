using PrimeTween;
using UnityEngine;
using UnityEngine.AI;

public class Gr_SecretDoor : Gr_BaseOpenableObject
{
    [SerializeField] private float offset = 3f;
    private NavMeshObstacle navMeshObstacle;
    private void Awake()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }
    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Default");
        start = transform.localPosition;
        end = start + direct * offset;
    }
    public override void CloseObj()
    {
        Tween.LocalPosition(transform, end, start, 1f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            isOpened = false;
            navMeshObstacle.carving = true;
        });
    }

    public override void OpenObj()
    {
        Tween.LocalPosition(transform, start, end, 1f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            isOpened = true;
            navMeshObstacle.carving = false;
        });
    }
}
