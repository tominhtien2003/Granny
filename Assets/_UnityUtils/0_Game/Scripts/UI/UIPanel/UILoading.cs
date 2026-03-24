using LitMotion;
using LitMotion.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;
public class UILoading : MonoBehaviour
{
    [Header("LOADING_PLAY")]
    [SerializeField] private Image bgPlay;
    [SerializeField] private Image process;
    [SerializeField] private Sprite[] spriteBg;

    [Header("LOADING_PANEL_FADE")]
    [SerializeField] private Image loadingFadeImage;
    [SerializeField] private float loadingHalfDuration;
#if UNITY_EDITOR

    private float _timeLoadingGame = 1;
#else
    private float _timeLoadingGame = 3;
#endif

    private bool _isLoadingPlayGame;
    private bool _isLoading;

    private MotionHandle m_loadingFadeHandle;
    private MotionHandle m_processHandle;

    private bool on_off_banner_in_loading;

    private void Awake()
    {
        on_off_banner_in_loading = PrefData.on_off_banner_in_loading;
    }


    public void CallLoadingPlayGame(int indexGame, Action actionHide, bool autoHide = true)
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowMrec();
#endif
        if (!on_off_banner_in_loading)
        {
#if ADS_AVAILABLE
            AdsAdapter.Instance.HideBanner();
#endif
        }
        if (_isLoadingPlayGame) return;
        _isLoadingPlayGame = true;
        bgPlay.gameObject.SetActive(true);

        m_processHandle.TryCancel();
        m_processHandle = LMotion.Create(0, 1f, _timeLoadingGame).WithOnComplete(() =>
        {
            actionHide?.Invoke();
            if (autoHide)
            {
                CallLoadingPlayGHide();
            }
        }).BindToFillAmount(process).AddTo(process);
    }

    public void CallLoadingShowFade(Action action, bool autoHide = true)
    {
        if (_isLoading) return;
        _isLoading = true;
        loadingFadeImage.gameObject.SetActive(true);
        m_loadingFadeHandle.TryCancel();
        m_loadingFadeHandle = LMotion.Create(0f, .5f, loadingHalfDuration).WithOnComplete(() =>
        {
            action?.Invoke();
            LMotion.Create(.5f, 1, loadingHalfDuration).WithOnComplete(() =>
            {
                if (autoHide)
                {
                    CallLoadingFadeHide();
                }
            }).BindToColorA(loadingFadeImage).AddTo(loadingFadeImage);
        }).BindToColorA(loadingFadeImage).AddTo(loadingFadeImage);
    }

    public void CallLoadingPlayGHide()
    {
        bgPlay.gameObject.SetActive(false);
        Hide();
#if ADS_AVAILABLE
	    AdsAdapter.Instance.HideMrec();
#endif
    }

    public void CallLoadingFadeHide()
    {
        m_loadingFadeHandle = LMotion.Create(1f, 0f, loadingHalfDuration).WithOnComplete(() =>
        {
            loadingFadeImage.gameObject.SetActive(false);
            Hide();
        }).BindToColorA(loadingFadeImage).AddTo(loadingFadeImage);
    }

    private void Hide()
    {
        _isLoadingPlayGame = false;
        _isLoading = false;
    }

}
