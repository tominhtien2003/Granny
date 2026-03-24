using System;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{


    public static IndicatorController Instance;
    
    [SerializeField] private Interact_Waypoint_Indicator clickButtonIndicator;
    
    // [SerializeField] private Waypoint_Indicator holdButtonIndicator;
    
    public ButtonClickIndicator _buttonClickIndicator;
    public List<ButtonClickIndicator> m_subButtonClickIndicators;
    // private IOS_ButtonHoldIndicator _buttonHoldIndicator;

    //private Interact_UIGame _uiGame;
    private int m_currentIndx;

    public void SetOffset(float o)
    {
        Vector3 p = _buttonClickIndicator.transform.localPosition;
        p.x = o;
        _buttonClickIndicator.transform.localPosition = p;

        for (int h = 0; h < m_currentAmount; h++)
        {
            p = m_subButtonClickIndicators[h].transform.localPosition;
            p.x = o;
            m_subButtonClickIndicators[h].transform.localPosition = p;
        }

    }
    public void Awake()
    {
        Instance = this;
        m_currentIndx = -1;
        
        //_uiGame = School_UIController.Instance.uiGame;
    }
    public int m_currentAmount = 0;
    public void SetSubButtonActives(int amount)
    {
        while (m_currentAmount > amount)
        {
            m_currentAmount--;
            m_subButtonClickIndicators[m_currentAmount].gameObject.SetActive(false);
            
        }

        while (m_currentAmount < amount)
        {
            m_subButtonClickIndicators[m_currentAmount].gameObject.SetActive(true);
            m_currentAmount++;
        }
    }
    public void SetSubButtonName(int id, string name)
    {
        m_subButtonClickIndicators[id].SetName(name);
    }
    public void ResetButtonPreset()
    {
        m_currentIndx = -1;
    }
    public void SetButtonNames(int i, int amount, List<string[]> preset)
    {
        if (m_currentIndx == i) return;
        m_currentIndx   = i;
        _buttonClickIndicator.SetName(preset[i][0]);

        SetSubButtonActives(amount - 1);
        for (int h = 1; h < amount; h++)
        {
            m_subButtonClickIndicators[h - 1].SetName(preset[i][h]);
        }
    }
    
    public Transform GetClickBtnTransform()
    {
        return _buttonClickIndicator.transform;
    }
    
    public void SetClickBtn(Transform tParent, ButtonClickIndicator indicator)
    {
        clickButtonIndicator.transform.SetParent(tParent);
        clickButtonIndicator.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    
    public void DisplaySubClickBtns(Transform tParent, Action action, bool iconAds, bool offUI = true, int id = 0)
    {
        var indicator = m_subButtonClickIndicators[id];
        indicator.Init(action, offUI, Color.white);
        indicator.Enable(iconAds);
        
    }
    public void DisplayClickBtn(Transform tParent, string nameIndicator, Action action, ButtonClickIndicator indicator, bool iconAds, bool offUI = true)
    {
        SetClickBtn(tParent, indicator);
        indicator.Init(nameIndicator, action, offUI, Color.white);
        clickButtonIndicator.onScreenGameObjectHide = false;
        indicator.Enable(iconAds);
    }
    public void DisplayClickBtn(Transform tParent, Action action, ButtonClickIndicator indicator, bool iconAds, bool offUI = true)
    {
        SetClickBtn(tParent, indicator);
        indicator.Init(action, offUI, Color.white);
        clickButtonIndicator.onScreenGameObjectHide = false;
        indicator.Enable(iconAds);
    }
    public void DisplayClickBtn(Transform tParent, string nameIndicator, Action action, bool offUI = true)
    {
        SetClickBtn(tParent);
        Color color = Color.white;

        _buttonClickIndicator.Init(nameIndicator, action, offUI, color);
        clickButtonIndicator.onScreenGameObjectHide = false;
        _buttonClickIndicator.Enable(false);
    }
    public void DisplayClickBtn(Transform tParent, Action action, bool offUI = true)
    {
        DisplayClickBtn(tParent, action, _buttonClickIndicator, false, offUI);
    }

    private void SetClickBtn(Transform tParent)
    {
       /* if (_buttonClickIndicator == null)
        {
            if (clickButtonIndicator.GameObjectIndicator == null)
            {
                this.LogError("click null obj");
                return;
            }
            var v = clickButtonIndicator.GameObjectIndicator.GetComponentsInChildren<ButtonClickIndicator>();
            _buttonClickIndicator = v[0];
            for (int i = 1; i < v.Length; i++) m_subButtonClickIndicators.Add(v[i]);
           
        }*/

        clickButtonIndicator.transform.SetParent(tParent);
        clickButtonIndicator.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
    }


    public void HideClickBtn(bool setParentNull = false)
    {
        if (setParentNull)
        {
            clickButtonIndicator.transform.SetParent(null);
            
        }
        _buttonClickIndicator.Disable(target:clickButtonIndicator,target => target.onScreenGameObjectHide = true);
    }
    // public void DisplayHoldBtn(Transform tParent, string nameIndicator, Action action, bool iconAds, bool offUI = true)
    // {
    //     if (_buttonHoldIndicator == null)
    //     {
    //         _buttonHoldIndicator = holdButtonIndicator.GameObjectIndicator.GetComponent<IOS_ButtonHoldIndicator>();
    //     }
    //     holdButtonIndicator.transform.SetParent(tParent);
    //     holdButtonIndicator.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
    //     _buttonHoldIndicator.Init(nameIndicator, action, offUI);
    //     holdButtonIndicator.onScreenGameObjectHide = false;
    //     _buttonHoldIndicator.Enable(iconAds);
    // }
    //
    // public void HideHoldBtn()
    // {
    //     _buttonHoldIndicator.Disable(() =>
    //     {
    //         holdButtonIndicator.onScreenGameObjectHide = true;
    //     });
    // }
}
