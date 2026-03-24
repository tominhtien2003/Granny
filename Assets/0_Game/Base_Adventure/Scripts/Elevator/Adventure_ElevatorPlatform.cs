using PrimeTween;
using UnityEngine;

public class Adventure_ElevatorPlatform : MonoBehaviour
{
    [SerializeField] float moveDuration = 10f;

    public Tween MoveToZ(float z)
    {
        return Tween.LocalPositionZ(
            transform,
            z,
            moveDuration,
            Ease.Linear
        );
    }
}
