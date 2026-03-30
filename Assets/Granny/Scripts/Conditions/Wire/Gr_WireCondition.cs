using UnityEngine;

public class Gr_WireCondition : Gr_BaseConditionInteractable
{
    [SerializeField] private GameObject wireBroken;
    protected override void ResolveConditions()
    {
        wireBroken.SetActive(true);
        gameObject.SetActive(false);
    }
}
