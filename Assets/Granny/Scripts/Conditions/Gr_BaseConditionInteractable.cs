using UnityEngine;

public abstract class Gr_BaseConditionInteractable : Gr_InteractableBase
{
    public bool IsSatisfied { get; protected set; }
    public SOItemData RequiredItem;
    public override void Interact()
    {
        var holdItem = Gr_PlayerHolder.Instance.GetCurrentItem();
        if (holdItem == null)
        {
            Gr_EventManager.Notify(new MissingRequiredItemEvent(RequiredItem.Name));
        }
        else
        {
            if (holdItem.ItemData.Type == RequiredItem.Type)
            {
                IsSatisfied = true;
                ResolveConditions();
            }
            else
            {
                Gr_EventManager.Notify(new MissingRequiredItemEvent(RequiredItem.Name));
            }
        }
    }
    
    public override void OnFocusEnter()
    {
        
    }

    public override void OnFocusExit()
    {
        
    }

    protected virtual void ResolveConditions()
    {
        Debug.Log("Resolved conditions");
    }
}
