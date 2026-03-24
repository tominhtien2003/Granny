using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<ButtonEffectLogic>().onClick.AddListener(OnPauseClicked);
    }

    private void OnPauseClicked()
    {
        HomeUIController.Instance.OpenSetting(true);
    }
}
