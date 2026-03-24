using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButtonGame : MonoBehaviour
{
    
    [SerializeField] private ButtonEffectLogic clickButton;
    private void Awake()
    {
        clickButton.onClick.AddListener(ActionClick);
    }

    private void ActionClick()
    {
        HomeUIController.Instance.skinPopup.Show();
    }
}
