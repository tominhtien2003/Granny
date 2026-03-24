using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_Switch : Adventure_InteractObj
{
    [SerializeField] private MonoBehaviour _activeObject;
    private IActiveBySwitch activeObject;

    protected override void Awake()
    {
        base.Awake();
        activeObject = _activeObject as IActiveBySwitch;

        if (activeObject == null)
            Debug.LogError($"{_activeObject.name} does not implement IActiveBySwitch", this);
    }

    public override void Interact()
    {
        base.Interact();
        _triggerBox.gameObject.SetActive(false);
        activeObject.Active();
    }
    public void ResetSwitch()
    {
        _triggerBox.gameObject.SetActive(true);
    }
}
