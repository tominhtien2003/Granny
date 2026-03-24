using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAJ_EggTutInjector : MonoBehaviour
{
    public ClimbFall m_slideState;
    private void Awake()
    {
        GetComponent<EggTutorialObject>().m_state = m_slideState;
    }
}
