using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGraphicToggler : MonoBehaviour
{
    public GameObject m_offObj;
    public GameObject m_onObj;

    public void Toggle(bool isOn)
    {
        m_offObj.SetActive(!isOn);
        m_onObj.SetActive(isOn);
    }
}
