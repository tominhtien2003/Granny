using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PRIMETWEEN
using PrimeTween;
#elif LIT_MOTION
using LitMotion;
using LitMotion.Extensions;
#elif  DOTWEEN
using DG.Tweening;
#endif


public class UIMenu : MonoBehaviour
{
    public ButtonEffectLogic rebirthButton, inventoryButton;
    public SkinButtonGame skinButton;
    public WingButtonGame wingButton;
    public ShieldButton shieldButton;
    public PauseButton settingButton;

#if PRIMETWEEN  || DOTWEEN
    Tween _shieldTween;
#endif
    public virtual void Init()
    {
        if (rebirthButton) rebirthButton.onClick.AddListener(OpenRebirth);
        if (inventoryButton) inventoryButton.onClick.AddListener(OpenInventory);
    }

    protected virtual void OpenInventory()
    {

    }

    protected virtual void OpenRebirth()
    {

    }

    protected virtual void OnDestroy()
    {
        ShieldComponent.isShield = false;
#if PRIMETWEEN  || DOTWEEN
        if (_shieldTween.IsActive())
        {
            _shieldTween.Kill();
        }
#endif
    }
}