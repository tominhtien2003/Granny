using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCupInjector : MonoBehaviour
{
    private CupScript m_cup;
    public SlideDown m_slideState;
    private void Awake()
    {
        m_cup = GetComponent<CupScript>();
        m_slideState.OnStateChanged += SetCup;
    }
    void SetCup(bool state)
    {
        if (!state) return;
        m_cup.EnableCollider(true);
    }
}
