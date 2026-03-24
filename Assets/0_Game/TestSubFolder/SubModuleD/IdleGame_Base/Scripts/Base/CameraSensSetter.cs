using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensSetter : MonoBehaviour
{
    public CinemachineCameraTouchController m_controller;
    protected virtual void Awake()
    {

        DataController.m_camSens = this;


    }
    private void Start()
    {
        SetSens(DataController.Setting_Sensitivity);
    }
    public virtual void SetSens(float sens)
    {
        m_controller.m_sensitivity = sens * .5f;
    }
}
