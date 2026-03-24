using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenHomeSkin : MonoBehaviour
{
    private void Awake()
    {
        if (HomeUIController.Instance == null) return;
        GetComponent<ButtonEffectLogic>().onClick.AddListener(HomeUIController.Instance.skinPopup.Show);
    }
}
