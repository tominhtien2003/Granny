using UnityEngine;
using PrimeTween;

public class G3_ChairElevator : MonoBehaviour
{
    public float duration = 3f;
    public Transform floor1Transform, floor2Transform;
    public bool isDone = false;

    public void StartMove(Transform des)
    {
        isDone = false;
        Tween.Position(transform, des.position, duration)
            .OnComplete(() => isDone = true);
    }
    public void MoveTo(FloorLevel goTo)
    {
        if (goTo == FloorLevel.Floor2)
            StartMove(floor1Transform);
        else
            StartMove(floor2Transform);
    }
    public void ResetElevator()
    {
        isDone = false;
        transform.position = floor1Transform.position;
    }
}
