using System.Collections.Generic;
using UnityEngine;

public abstract class Gr_BaseOpenableObject : Gr_InteractableBase
{
    [SerializeField] private List<Gr_BaseConditionInteractable> conditions = new List<Gr_BaseConditionInteractable>();
    public Collider col;
    public Vector3 direct;

    protected Vector3 start, end;
    protected bool isOpened = false;
    protected bool isMoving = false;
    
    protected bool isSatisfiedAllCondition = false;

    private const string interactableLayer = "Interactable";
    private const string defaultLayer = "Default";

    protected virtual void OnEnable()
    {
        Gr_EventManager.AddListener<ConditionSatisfiedEvent>(HandleConditionSatisfied);
    }
    protected virtual void OnDisable()
    {
        Gr_EventManager.RemoveListener<ConditionSatisfiedEvent>(HandleConditionSatisfied);
    }
    protected virtual void Start()
    {
        if (conditions == null || conditions.Count == 0)
        {
            isSatisfiedAllCondition = true;
            SetInteractableState(interactableLayer);
        }
        else
        {
            isSatisfiedAllCondition = false;
            SetInteractableState(defaultLayer);
        }
    }
    private void SetInteractableState(string nameLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayer);
    }
    private void HandleConditionSatisfied(ConditionSatisfiedEvent e)
    {
        if (conditions == null || conditions.Count == 0) return;
        foreach (var condi in conditions)
        {
            if (!condi.IsSatisfied)
            {
                isSatisfiedAllCondition = false;
                return;
            }
        }
        isSatisfiedAllCondition = true;
        SetInteractableState(interactableLayer);
    }
    public override void OnFocusEnter()
    {
        
    }
    public override void Interact()
    {
        if (!isSatisfiedAllCondition) return;
        if (isMoving) return;
        isMoving = true;
        if (isOpened)
        {
            CloseObj();
        }
        else
        {
            OpenObj();
        }
    }
    public override void OnFocusExit()
    {
        
    }
    public abstract void OpenObj();
    public abstract void CloseObj();
}
