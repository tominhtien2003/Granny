using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInjector : MonoBehaviour
{
    public ClimbFall m_fallState;
    private CupScript m_cup;
    private void Awake()
    {
        m_fallState.OnGroundHitEvent += SetColl; 
        m_cup = GetComponent<CupScript>();
    }
    void SetColl()
    {
        m_cup.EnableCollider(true);
    }
}
