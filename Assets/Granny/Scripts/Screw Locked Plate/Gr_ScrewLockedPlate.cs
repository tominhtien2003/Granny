using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_ScrewLockedPlate : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private List<Gr_BaseConditionInteractable> conditions;

    private bool isSatisfiedAllCondition = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (conditions == null || conditions.Count == 0)
        {
            isSatisfiedAllCondition = true;
            rb.isKinematic = false;
        }
        else
        {
            isSatisfiedAllCondition = false;
        }
    }
    protected virtual void OnEnable()
    {
        Gr_EventManager.AddListener<ConditionSatisfiedEvent>(HandleConditionSatisfied);
    }
    protected virtual void OnDisable()
    {
        Gr_EventManager.RemoveListener<ConditionSatisfiedEvent>(HandleConditionSatisfied);
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
        rb.isKinematic = false;
    }
}
