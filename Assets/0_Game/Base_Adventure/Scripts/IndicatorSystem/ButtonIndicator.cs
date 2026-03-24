using System;
using TMPro;
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

public abstract class ButtonIndicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroupDisplay;
    [SerializeField] private RectTransform rectText;
    public TMP_Text nameText;
    [SerializeField] protected GameObject adsIcon;
    [SerializeField] protected Image fill;

    private float _anchorXInit;
    private bool _offUi;
    private Action _onCompleted;

#if LIT_MOTION
    private CompositeMotionHandle m_handle = new CompositeMotionHandle();
#endif


    private void Start()
    {
        _anchorXInit = rectText.anchoredPosition.x;
    }
    public void SetName(string nameDisplay)
    {
        nameText.SetText(nameDisplay);
    }
    public virtual void Init(string nameDisplay, Action onCompleted, bool offUi, Color colorText)
    {
        _offUi = offUi;
        fill.fillAmount = 0;
        _onCompleted = onCompleted;
        nameText.SetText(nameDisplay);
        nameText.color = colorText;
    }
    public virtual void Init(Action onCompleted, bool offUi, Color colorText)
    {
        _offUi = offUi;
        fill.fillAmount = 0;
        _onCompleted = onCompleted;
        nameText.color = colorText;
    }

    public void Enable(bool displayAds)
    {
        //adsIcon.SetActive(displayAds);

#if LIT_MOTION
        m_handle.Cancel();
#endif
        if (canvasGroupDisplay == null)
        {

            return;
        }

#if PRIMETWEEN
        canvasGroupDisplay.DOKill();
        rectText.DOKill();
        Tween.Alpha(canvasGroupDisplay, 0, 1, .3f);
        Tween.UIAnchoredPositionX(rectText, 0, _anchorXInit, .3f);
#elif LIT_MOTION
        LMotion.Create(0, 1f, .3f).BindToAlpha(canvasGroupDisplay).AddTo(m_handle);

        LMotion.Create(0, _anchorXInit, .3f).BindToAnchoredPositionX(rectText).AddTo(m_handle);
#elif DOTWEEN
        canvasGroupDisplay.DOKill();
        rectText.DOKill();
tu code di
#endif
    }

    public void Disable<T>(T target, Action<T> onCompleted) where T : class
    {
        if (canvasGroupDisplay == null)
        {
            return;
        }
        canvasGroupDisplay.DOKill();
        rectText.DOKill();
        if (canvasGroupDisplay.alpha == 0)
        {
            onCompleted?.Invoke(target);
            return;
        }
        Tween.Alpha(canvasGroupDisplay, 0, .3f);
        if (onCompleted != null) Tween.UIAnchoredPositionX(rectText, 0, .3f).OnComplete(target, onCompleted);
        else Tween.UIAnchoredPositionX(rectText, 0, .3f);
    }
    public void Disable(Action onCompleted)
    {
#if LIT_MOTION
        m_handle.Cancel();
#endif
        if (canvasGroupDisplay == null)
        {
            return;
        }
#if PRIMETWEEN
        canvasGroupDisplay.DOKill();
        rectText.DOKill();
        if (canvasGroupDisplay.alpha == 0)
        {
            onCompleted?.Invoke();
            return;
        }
        Tween.Alpha(canvasGroupDisplay, 0, .3f);
        if (onCompleted != null) Tween.UIAnchoredPositionX(rectText, 0, .3f).OnComplete(onCompleted);
        else Tween.UIAnchoredPositionX(rectText, 0, .3f);
#elif LIT_MOTION
        LMotion.Create(canvasGroupDisplay.alpha, 0, .3f).BindToAlpha(canvasGroupDisplay).AddTo(m_handle);
        LMotion.Create(rectText.anchoredPosition.x, 0, .3f).WithOnComplete(() =>
        {
            onCompleted?.Invoke();
        }).BindToAnchoredPositionX(rectText).AddTo(m_handle);
#elif DOTWEEN
        canvasGroupDisplay.DOKill();
        rectText.DOKill();
tu code di
#endif
    }

    protected virtual void ResetValue()
    {
        fill.fillAmount = 0;
    }


    protected void Completed()
    {
        ResetValue();
        if (_offUi)
        {
            canvasGroupDisplay.alpha = 0;
        }
        _onCompleted?.Invoke();
    }
    private void OnDisable()
    {

#if PRIMETWEEN || DOTWEEN
        if (canvasGroupDisplay)
        {
            canvasGroupDisplay.DOKill();
        }
        if (rectText)
        {
            rectText.DOKill();
        }
#elif LIT_MOTION
        m_handle.Cancel();
#endif
    }
}
