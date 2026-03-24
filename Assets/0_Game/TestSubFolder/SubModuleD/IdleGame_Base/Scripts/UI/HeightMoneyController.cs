using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeightMoneyController : MoneyController
{

    //[SerializeField] private ClimbFall m_fallState;
    [SerializeField] private float m_heightPerMoney = 10f;
    

    public static HeightMoneyController Instance;
    public float m_currentHeight;

    public float m_mapMultiplier = 1;
    private void Awake()
    {
        //m_climbState.OnStateChanged += OnClimbStateSwitched;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    protected override void CalculateBaseMoney(ref double baseMoney)
    {
        if (m_currentHeight < .89f)
        {
            baseMoney = 0;
            return;
        }
        baseMoney = m_mapMultiplier * m_currentHeight / m_heightPerMoney;

    }
   
   
}
