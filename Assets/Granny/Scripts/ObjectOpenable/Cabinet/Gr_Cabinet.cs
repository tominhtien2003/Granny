using PrimeTween;
public class Gr_Cabinet : Gr_BaseOpenableObject
{
    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }

    public override void OpenObj()
    {
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
        });
    }

    public override void CloseObj()
    {
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
        });
    }
}
