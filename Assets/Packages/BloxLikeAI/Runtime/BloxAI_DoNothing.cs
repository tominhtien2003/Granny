using UnityEngine;

public class BloxAI_DoNothing : BaseAIBloxState
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SetMovementAction(Vector3.zero);
    }
}
