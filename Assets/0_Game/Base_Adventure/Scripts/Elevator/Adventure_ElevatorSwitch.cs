using UnityEngine;

public class Adventure_ElevatorSwitch : Adventure_InteractObj
{
    [SerializeField] Adventure_ElevatorController elevator;

    public override void Interact()
    {
        base.Interact();
        elevator.CallFromSwitch();
        _triggerBox.gameObject.SetActive(false);
    }

    public void ResetSwitch()
    {
        _triggerBox.gameObject.SetActive(true);
    }
}
