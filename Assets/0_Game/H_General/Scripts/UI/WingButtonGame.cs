using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingButtonGame : MonoBehaviour
{
    [SerializeField] private ButtonEffectLogic clickButton;
    private void Awake()
    {
        clickButton.onClick.AddListener(ActionClick);
    }

    private void ActionClick()
    {
        HomeUIController.Instance.wingPopup.Show();
    }
}
