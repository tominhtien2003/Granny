using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    public static ShieldComponent Instance;

    [SerializeField] private GameObject shieldEffect;

    public static bool isShield;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleEffect(bool isActive)
    {
        isShield = isActive;
        shieldEffect.SetActive(isActive);
    }

    private void OnDestroy()
    {
        isShield = false;
    }
}
