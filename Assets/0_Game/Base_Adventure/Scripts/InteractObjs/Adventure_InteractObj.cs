using EPOOutline;
using System;
using UnityEngine;

public abstract class Adventure_InteractObj : MonoBehaviour
{
    [SerializeField] protected TriggerBox _triggerBox;
    [SerializeField] protected Outlinable[] _outlines;     

    protected bool _playerInside = false;
    [SerializeField] protected string actionName = "Interact";
    [SerializeField] protected Transform indicatorTransform;
    protected virtual void Awake()
    {
        _triggerBox.onTriggerEnter += OnTriggerEnter_Custom;
        _triggerBox.onTriggerExit += OnTriggerExit_Custom;
        _triggerBox.onTriggerStay += OnTriggerStay_Custom;
        G4InteractAction = Interact;
    }
    protected virtual void Start()
    {
        if(indicatorTransform == null)
        {
            indicatorTransform = transform;
        }
    }
    protected virtual void OnTriggerEnter_Custom(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            ToggleOutline(true);
            IndicatorController.Instance.DisplayClickBtn(indicatorTransform, actionName, G4InteractAction, false);
        }
    }

    protected virtual void OnTriggerExit_Custom(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            ToggleOutline(false);
            IndicatorController.Instance.HideClickBtn(true);
        }
    }
    protected virtual void OnTriggerStay_Custom(Collider other)
    {

    }
    protected void ToggleOutline(bool enable)
    {
        foreach (var outline in _outlines)
        {
            outline.enabled = enable;
        }
    }

    public Action G4InteractAction;
    public virtual void Interact()
    {
        if (!_playerInside) return;
        ToggleOutline(false);
        IndicatorController.Instance.HideClickBtn(true);
    }
}
